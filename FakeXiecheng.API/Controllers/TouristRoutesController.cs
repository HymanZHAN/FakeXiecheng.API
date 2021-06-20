using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dto;
using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.ResourceParameters;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
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

        public TouristRoutesController(ITouristRouteRepository routeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _touristRouteRepository = routeRepository;
        }

        [HttpHead]
        [HttpGet]
        public async Task<IActionResult> GetTouristRoutes([FromQuery] TouristRouteResourceParameters parameters)
        {
            var routesFromRepo = await _touristRouteRepository.GetTouristRoutesAsync(parameters.Keyword,
                              parameters.RatingComparison,
                              parameters.RatingValue);

            if (routesFromRepo == null || !routesFromRepo.Any())
            {
                return NotFound("没有旅游路线");
            }

            var touristRoutes = _mapper.Map<IEnumerable<TouristRouteDto>>(routesFromRepo);

            return Ok(touristRoutes);
        }

        [HttpGet("{routeId:Guid}", Name = "GetTouristRouteById")]
        [HttpHead("{routeId:Guid}")]
        public async Task<IActionResult> GetTouristRouteById(Guid routeId)
        {
            var routeFromRepo = await _touristRouteRepository.GetTouristRouteAsync(routeId);

            if (routeFromRepo == null)
            {
                return NotFound($"没有对应{routeId}的旅游路线");
            }

            var touristRouteDto = _mapper.Map<TouristRouteDto>(routeFromRepo);

            return Ok(touristRouteDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto routeForCreateionDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(routeForCreateionDto);
            await _touristRouteRepository.AddTouristRouteAsync(touristRouteModel);
            await _touristRouteRepository.SaveAsync();
            var result = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(nameof(GetTouristRouteById), new { routeId = touristRouteModel.Id }, result);
        }

        [HttpPut("{routeId:Guid}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateTouristRoute(
            [FromRoute] Guid routeId,
            [FromBody] TouristRouteForUpdateDto routeForUpdateDto)
        {
            if (!await _touristRouteRepository.CheckIfTouristRouteExistAsync(routeId))
            {
                return NotFound("路由路线找不到");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(routeId);
            // 1. 映射到dto
            // 2. 更新dto
            // 3. 映射到model
            var updatedModel = _mapper.Map(routeForUpdateDto, touristRouteFromRepo);

            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<TouristRouteDto>(updatedModel));
        }

        [HttpPatch("{routeId:Guid}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PartiallyUpdateTouristRoute(
            [FromRoute] Guid routeId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            if (!await _touristRouteRepository.CheckIfTouristRouteExistAsync(routeId))
            {
                return NotFound("路由路线找不到");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(routeId);
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }

            var result = _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<TouristRouteDto>(result));
        }

        [HttpDelete("{routeId:Guid}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteTouristRoute([FromRoute] Guid routeId)
        {
            if (!await _touristRouteRepository.CheckIfTouristRouteExistAsync(routeId))
            {
                return NotFound("旅游路线找不到");
            }
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(routeId);
            _touristRouteRepository.DeleteTouristRoute(touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("({routeIds})")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteRoutes([ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<Guid> routeIds)
        {
            if (routeIds == null)
            {
                return BadRequest();
            }

            var touristRoutes = await _touristRouteRepository.GetTouristRoutesByIdListAsync(routeIds);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutes);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
    }
}