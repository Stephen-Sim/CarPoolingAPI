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
                x.Status,

                DisplayNumOfPerson = $" ({(x.TripRequests != null ? x.TripRequests.Count : 0)}🤵)",
                DiplayFromAddress = x.FromAddress.Length > 35 ? x.FromAddress.Substring(0, 30) + "...." : x.FromAddress,
                DisplayToAddress = x.ToAddress.Length > 35 ? x.ToAddress.Substring(0, 30) + "...." : x.ToAddress,
                DisplayTime = $"{x.Date.ToString("dd/MM/yyyy")} {x.Time} - "
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
                    Status = "Searching",
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

        [HttpPost("GetTripsRequestByTrip"), Authorize(Roles = "Driver")]
        public IActionResult GetTripsRequestByTripId([FromHeader] string token, int tripId)
        {
            var jwt = new JwtSecurityToken(token);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

            if (driver == null)
                return Unauthorized();

            var trip = context.Trips.FirstOrDefault(x => x.Id == tripId);

            if (trip == null)
                return BadRequest();

            var midpoint = FindMidPoint((double)trip.FromLatitude, (double)trip.FromLongitude, (double)trip.ToLatitude, (double)trip.ToLongitude);

            var requests = context.Requests.ToList().Where(x =>
                CalculateDistance(new Position((double)x.FromLatitude, (double)x.FromLongitude), midpoint) <= 10.0d &&
                x.Date.AddTicks(x.Time.Ticks) <= trip.Date.AddTicks(x.Time.Ticks + TimeSpan.TicksPerHour) &&
                x.Date.AddTicks(x.Time.Ticks) >= trip.Date.AddTicks(x.Time.Ticks - TimeSpan.TicksPerHour));

            return Ok(requests);
        }

        private Position FindMidPoint(double lat1, double lon1, double lat2, double lon2)
        {
            double dLon = (Math.PI / 180) * (lon2 - lon1);

            //convert to radians
            lat1 = (Math.PI / 180) * lat1;
            lat2 = (Math.PI / 180) * lat2;
            lon1 = (Math.PI / 180) * lon1;

            double Bx = Math.Cos(lat2) * Math.Cos(dLon);
            double By = Math.Cos(lat2) * Math.Sin(dLon);
            double lat3 = Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2), Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By));
            double lon3 = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);

            return new Position((180 / Math.PI) * lat3, (180 / Math.PI) * lon3);
        }

        private double CalculateDistance(Position point1, Position point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6371 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
