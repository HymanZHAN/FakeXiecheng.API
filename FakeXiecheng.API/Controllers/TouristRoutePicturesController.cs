using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dto;
using FakeXiecheng.API.Helpers;
using FakeXiecheng.API.Models;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/touristRoutes/{routeId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController : ControllerBase
    {
        private readonly ITouristRouteRepository _repo;
        private readonly IMapper _mapper;

        public TouristRoutePicturesController(ITouristRouteRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetPictureForTouristRoute(Guid routeId)
        {
            if (!await _repo.CheckIfTouristRouteExistAsync(routeId))
            {
                return NotFound("旅游路线不存在");
            }

            var picturesFromRepo = await _repo.GetPicturesByTouristRouteIdAsync(routeId);
            if (picturesFromRepo == null || picturesFromRepo.Count() <= 0)
            {
                return NotFound("照片不存在");
            }

            var pictures = _mapper.Map<IEnumerable<TouristRoutePictureDto>>(picturesFromRepo);
            return Ok(pictures);
        }

        [HttpGet("{pictureId:int}", Name = "GetPicture")]
        public async Task<IActionResult> GetPicture(Guid routeId, int pictureId)
        {
            if (!await _repo.CheckIfTouristRouteExistAsync(routeId))
            {
                return NotFound("旅游路线不存在");
            }

            var pictureFromRepo = await _repo.GetPictureAsync(pictureId);
            if (pictureFromRepo == null)
            {
                return NotFound("图片不存在");
            }

            var picture = _mapper.Map<TouristRoutePictureDto>(pictureFromRepo);
            return Ok(picture);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTouristRoutePicture(
            [FromRoute] Guid routeId,
            [FromBody] TouristRoutePictureForCreationDto routePictureForCreationDto
        )
        {
            if (!await _repo.CheckIfTouristRouteExistAsync(routeId))
            {
                return NotFound("旅游路线不存在");
            }

            var pictureModel = _mapper.Map<TouristRoutePicture>(routePictureForCreationDto);
            _repo.AddTouristRoutePicture(routeId, pictureModel);
            await _repo.SaveAsync();

            var result = _mapper.Map<TouristRoutePictureDto>(pictureModel);
            return CreatedAtRoute(
                "GetPicture",
                new
                {
                    touristRouteId = pictureModel.TouristRouteId,
                    pictureId = pictureModel.Id
                },
                result
            );
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePicture([FromRoute] Guid routeId, [FromRoute] int pictureId)
        {
            if (!await _repo.CheckIfTouristRouteExistAsync(routeId))
            {
                return NotFound("旅游路线不存在");
            }

            var picture = await _repo.GetPictureAsync(pictureId);
            _repo.DeleteTouristRoutePicture(picture);
            await _repo.SaveAsync();

            return NoContent();
        }
    }
}