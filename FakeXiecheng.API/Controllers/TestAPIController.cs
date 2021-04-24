using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/shoudongapi")]
    public class TestAPIController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var result = new string[] { };
            return new string[] { "value1", "value2" };
        }
        
        [HttpPost]
        public IEnumerable<string> GetString()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
