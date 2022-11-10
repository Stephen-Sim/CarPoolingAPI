using CarPoolingAPI.Data;
using CarPoolingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarPoolingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly DataContext context;

        public PassengerController(DataContext context)
        {
            this.context = context;
        }

        [HttpPost("test")]
        public IActionResult test(string abc)
        {
            return Ok(abc);
        }

    }
}
