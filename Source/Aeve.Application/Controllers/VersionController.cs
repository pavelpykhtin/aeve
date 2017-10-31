using System;
using Microsoft.AspNetCore.Mvc;

namespace Aeve.Application.Controllers
{
    [Route("/api/version")]
    public class VersionController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new Version(0, 1, 0));
        }
    }
}