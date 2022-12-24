using AutoMapper;
using CarPoolingAPI.Data;
using CarPoolingAPI.Dto;
using CarPoolingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;

namespace CarPoolingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public DataContext context { get; }
        public IMapper mapper { get; }

        public RequestController(DataContext context, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpGet("GetRequests"), Authorize(Roles = "Passenger")]
        public IActionResult GetRequestsByPassengerToken([FromHeader] string token)
        {
            var jwt = new JwtSecurityToken(token);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            var passenger = context.Passengers.FirstOrDefault(x => x.Username == username);

            if (passenger == null)
                return Unauthorized();

            var requests = context.Requests.Where(x => x.PassengerId == passenger.Id).OrderBy(x => x.Date).ThenBy(x => x.Time).ToList().Select(x => new
            {
                x.Id,
                x.RequestNumber,
                x.FromLatitude,
                x.FromLongitude,
                x.FromAddress,
                x.ToLatitude,
                x.ToLongitude,
                x.ToAddress,
                x.NumberOfPassengers,
                x.Date,
                x.Time,
                x.Charges,
                x.Status,
                DisplayNumOfPerson = $" ({x.NumberOfPassengers}🤵)",
                DiplayFromAddress = x.FromAddress.Length > 35 ? x.FromAddress.Substring(0, 30) + "...." : x.FromAddress,
                DisplayToAddress = x.ToAddress.Length > 35 ? x.ToAddress.Substring(0, 30) + "...." : x.ToAddress,
                DisplayTime = $"{x.Date.ToString("dd/MM/yyyy")} {x.Time} - "
            });


            if (requests != null)
            {
                return Ok(requests);
            }

            return BadRequest();
        }

        [HttpPost("Create"), Authorize(Roles = "Passenger")]
        public IActionResult Create([FromHeader] string token, [FromBody] RequestDto request)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
                string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

                var passenger = context.Passengers.FirstOrDefault(x => x.Username == username);

                if (passenger == null)
                    return Unauthorized();

                var rand = new Random();

                var newRequest = new Request
                {
                    RequestNumber = $"CP{rand.Next(1, 99999).ToString("00000")}",
                    FromLatitude = (decimal)request.FromLatitude,
                    FromLongitude = (decimal)request.FromLongitude,
                    FromAddress = request.FromAddress,
                    ToLatitude = (decimal)request.ToLatitude,
                    ToLongitude = (decimal)request.ToLongitude,
                    ToAddress = request.ToAddress,
                    NumberOfPassengers = (int)request.NumberOfPassengers,
                    Date = (DateTime)request.Date,
                    Time = (TimeSpan)request.Time,
                    Charges = (Decimal)request.Charges,
                    Status = "Searching",
                    PassengerId = passenger.Id
                };

                this.context.Add(newRequest);
                this.context.SaveChanges();

                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest();
            }
        }
    }
}
