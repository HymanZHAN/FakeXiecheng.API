using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dto;
using FakeXiecheng.API.ResourceParameters;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;

        public OrdersController(
            IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContext,
            ITouristRouteRepository touristRouteRepository,
            IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _httpContext = httpContext;
            _mapper = mapper;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationResourceParameters parameters)
        {
            // 1. get current user
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // 2. get orders by user id
            var orders = await _touristRouteRepository.GetOrdersByUserId(userId, parameters.PageNumber, parameters.PageSize);

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId:Guid}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            // 1. get current user
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // 2. get order by order id
            var order = await _touristRouteRepository.GetOrderById(orderId);

            return Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpPost("{orderId:Guid}/placeOrder")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PlaceOrder([FromRoute] Guid orderId)
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var order = await _touristRouteRepository.GetOrderById(orderId);
            order.ProcessPayment();
            await _touristRouteRepository.SaveAsync();

            var httpClient = _clientFactory.CreateClient();
            var url = @"http://123.56.149.216/api/FakePaymentProcess?icode={0}&orderNumber={1}&returnFaut={2}";
            var response = await httpClient.PostAsync(
                string.Format(url, "9BEF48A349F9415B", order.Id, false),
                null);

            bool isApproved = false;
            string transactionMetadata = "";
            if (response.IsSuccessStatusCode)
            {
                transactionMetadata = await response.Content.ReadAsStringAsync();
                var jsonElement = JsonDocument.Parse(transactionMetadata);
                isApproved = jsonElement.RootElement.GetProperty("approved").GetBoolean();
            }

            if (isApproved)
            {
                order.ApprovePayment();
            }
            else
            {
                order.RejectPayment();
            }
            order.TransactionMetaData = transactionMetadata;
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<OrderDto>(order));
        }
    }
}