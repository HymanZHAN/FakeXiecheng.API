using AutoMapper;
using FakeXiecheng.API.Dto;
using FakeXiecheng.API.ResourceParameters;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

            if (routesFromRepo == null || routesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }

            var touristRoutes = _mapper.Map<IEnumerable<TouristRouteDto>>(routesFromRepo);

            return Ok(touristRoutes);
        }

        [HttpGet("{touristRouteId:Guid}")]
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
    }
}