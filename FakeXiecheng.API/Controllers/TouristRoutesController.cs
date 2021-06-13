using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FakeXiecheng.API.Dto;
using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.ResourceParameters;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TouristRoutesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _mapper = mapper;
            _touristRouteRepository = touristRouteRepository;
        }

        [HttpHead]
        [HttpGet]
        public IActionResult GetTouristRoutes([FromQuery] TouristRouteResourceParameters parameters)
        {
            var routesFromRepo = _touristRouteRepository.GetTouristRoutes(parameters.Keyword,
                              parameters.RatingComparison,
                              parameters.RatingValue);

            if (routesFromRepo == null || !routesFromRepo.Any())
            {
                return NotFound("没有旅游路线");
            }

            var touristRoutes = _mapper.Map<IEnumerable<TouristRouteDto>>(routesFromRepo);

            return Ok(touristRoutes);
        }

        [HttpGet("{touristRouteId:Guid}", Name = "GetTouristRouteById")]
        [HttpHead("{touristRouteId:Guid}")]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var routeFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);

            if (routeFromRepo == null)
            {
                return NotFound($"没有对应{touristRouteId}的旅游路线");
            }

            var touristRouteDto = _mapper.Map<TouristRouteDto>(routeFromRepo);

            return Ok(touristRouteDto);
        }

        [HttpPost]
        public IActionResult CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreateionDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreateionDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            _touristRouteRepository.Save();
            var result = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute("GetTouristRouteById", new { touristRouteId = touristRouteModel.Id }, result);
        }

        [HttpPut("{touristRouteId:Guid}")]
        public IActionResult UpdateTouristRoute(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            if (!_touristRouteRepository.CheckIfTouristRouteExist(touristRouteId))
            {
                return NotFound("路由路线找不到");
            }

            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            // 1. 映射到dto
            // 2. 更新dto
            // 3. 映射到model
            var updatedModel = _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);

            _touristRouteRepository.Save();

            return Ok(_mapper.Map<TouristRouteDto>(updatedModel));
        }

        [HttpPatch("{touristRouteId:Guid}")]
        public IActionResult PartiallyUpdateTouristRoute(
            [FromRoute] Guid touristRouteId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            if (!_touristRouteRepository.CheckIfTouristRouteExist(touristRouteId))
            {
                return NotFound("路由路线找不到");
            }

            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }

            var result = _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            _touristRouteRepository.Save();

            return Ok(_mapper.Map<TouristRouteDto>(result));
        }

        [HttpDelete("{touristRouteId:Guid}")]
        public IActionResult DeleteTouristRoute([FromRoute] Guid touristRouteId)
        {
            if (!_touristRouteRepository.CheckIfTouristRouteExist(touristRouteId))
            {
                return NotFound("旅游路线找不到");
            }
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRouteFromRepo);
            _touristRouteRepository.Save();
            return NoContent();
        }

        [HttpDelete("({touristRouteIds})")]
        public IActionResult DeleteRoutes([ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<Guid> touristRouteIds)
        {
            if (touristRouteIds == null)
            {
                return BadRequest();
            }

            var touristRoutes = _touristRouteRepository.GetTouristRoutesByIdList(touristRouteIds);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutes);
            _touristRouteRepository.Save();

            return NoContent();
        }
    }
}