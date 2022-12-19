using AutoMapper;
using CarPoolingAPI.Data;
using CarPoolingAPI.Dto;
using CarPoolingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace CarPoolingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public DataContext context { get; }
        public IMapper mapper { get; }

        public TripController(DataContext context, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpGet("GetTrips"), Authorize(Roles = "Driver")]
        public IActionResult GetTripByPassengerToken([FromHeader] string token)
        {
            var jwt = new JwtSecurityToken(token);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

            if (driver == null)
                return Unauthorized();

            var trips = context.Trips.Where(x => x.Vehicle.DriverId == driver.Id).OrderBy(x => x.Date).ThenBy(x => x.Time).ToList().Select(x => new
            {
                x.TripNumber,
                x.FromLatitude,
                x.FromLongitude,
                x.FromAddress,
                x.ToLatitude,
                x.ToLongitude,
                x.ToAddress,
                x.Date,
                x.Time,
                x.VehicleId,

                DisplayNumOfPerson = $" ({(x.TripRequests != null ? x.TripRequests.Count : 0)}🤵)",
                DiplayFromAddress = x.FromAddress.Length > 35 ? x.FromAddress.Substring(0, 30) + "...." : x.FromAddress,
                DisplayToAddress = x.ToAddress.Length > 35 ? x.ToAddress.Substring(0, 30) + "...." : x.ToAddress,
                DisplayTime = $"{x.Date.ToString("dd/MM/yyyy")} {x.Time} - ",
                Status = new Func<string>(() =>
                {
                    if (x.Status == "Canceled")
                    {
                        return "Canceled";
                    }

                    if (x.Status == "Pending" && x.Date.Add(x.Time) < DateTime.Now)
                    {
                        return "Coming Soon";
                    }

                    if (x.Status == "Pending" && x.Date.Add(x.Time) >= DateTime.Now)
                    {
                        return "Pending";
                    }

                    if (x.Status == "Completed" && x.Date > DateTime.Now)
                    {
                        return "Completed";
                    }

                    return "";
                })()
            });

            if (trips != null)
            {
                return Ok(trips);
            }

            return BadRequest();
        }

        [HttpPost("Create"), Authorize(Roles = "Driver")]
        public IActionResult Create([FromHeader] string token, [FromBody] TripDto trip)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
                string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

                var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

                if (driver == null)
                    return Unauthorized();

                var rand = new Random();

                var newTrip = new Trip
                {
                    TripNumber = $"TCP{rand.Next(1, 99999).ToString("00000")}",
                    FromLatitude = (decimal)trip.FromLatitude,
                    FromLongitude = (decimal)trip.FromLongitude,
                    FromAddress = trip.FromAddress,
                    ToLatitude = (decimal)trip.ToLatitude,
                    ToLongitude = (decimal)trip.ToLongitude,
                    ToAddress = trip.ToAddress,
                    Date = (DateTime)trip.Date,
                    Time = (TimeSpan)trip.Time,
                    Status = "Pending",
                    VehicleId = (int)trip.VehicleId
                };

                this.context.Add(newTrip);
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
