using CarPoolingAPI.Data;
using CarPoolingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace CarPoolingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        public DataContext context { get; }
        public IConfiguration configuration { get; }

        public ChatController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpGet("GetChatsByTripRequestId"), Authorize(Roles = "Driver")]
        public IActionResult GetChatsByTripRequestId([FromHeader] string token, int tripRequestId)
        {
            var jwt = new JwtSecurityToken(token);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

            if (driver == null)
                return Unauthorized();

            var chats = context.Chats.Where(x => x.TripRequestId == tripRequestId).OrderBy(x => x.DateTime).Select(x => new
            {
                x.Message,
                IsYourMessage = x.IsDriverMessage ? true : false,
                IsNotYourMessage = x.IsDriverMessage ? false : true,
            }).ToList();

            return Ok(chats);
        }

        [HttpGet("GetChatsByRequestId"), Authorize(Roles = "Passenger")]
        public IActionResult GetChatsByRequestId([FromHeader] string token, int requestId)
        {
            var jwt = new JwtSecurityToken(token);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            var passenger = context.Passengers.FirstOrDefault(x => x.Username == username);

            if (passenger == null)
                return Unauthorized();

            var tripRequest = context.TripRequests.FirstOrDefault(x => x.RequestId == requestId);

            var chats = context.Chats.Where(x => x.TripRequestId == tripRequest.Id).OrderBy(x => x.DateTime).Select(x => new
            {
                x.Message,
                IsYourMessage = x.IsDriverMessage ? false : true,
                IsNotYourMessage = x.IsDriverMessage ? true : false
            }).ToList();

            return Ok(chats);
        }
    }
}
