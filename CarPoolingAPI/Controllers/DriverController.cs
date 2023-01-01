using CarPoolingAPI.Data;
using CarPoolingAPI.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace CarPoolingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        public DataContext context { get; }
        public IConfiguration configuration { get; }

        public DriverController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpGet("GetDriverInfo"), Authorize(Roles = "Driver")]
        public IActionResult GetDriverInfo([FromHeader] string token)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
                string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

                var driver = context.Drivers.FirstOrDefault(x => x.Username == username);
                if (driver == null)
                    return Unauthorized();

                var rating = context.TripRequests.Include(x => x.Trip.Vehicle.Driver).Where(x => x.Trip.Vehicle.DriverId == driver.Id).Average(x => x.Rating);
                var earned = context.TripRequests.Include(x => x.Request).ThenInclude(x => x.TripRequest.Trip.Vehicle.Driver).Where(x => x.Trip.Vehicle.DriverId == driver.Id).Sum(x => x.Request.Charges);

                var info = new
                {
                    DriverName = driver.Username,
                    DriverProfile = driver.ProfileImage,
                    Rating = rating?.ToString("0.00") + " ⭐",
                    TotalEarned = earned.ToString("0.00")
                };

                return Ok(info);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest();
            }
        }

        [HttpGet("GetStatInfo"), Authorize(Roles = "Driver")]
        public IActionResult GetStatInfo([FromHeader] string token, int schecduleId)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
                string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

                var driver = context.Drivers.FirstOrDefault(x => x.Username == username);
                if (driver == null)
                    return Unauthorized();

                var ifExist = context.Requests
                    .Include(x => x.TripRequest.Trip.Vehicle.Driver)
                    .Where(x => x.TripRequest.Trip.Vehicle.DriverId == driver.Id && x.TripRequest.Trip.Status == "Completed").ToList();

                if (ifExist == null)
                    return Ok(new
                    {
                        InfoNoResult = true
                    });

                if (schecduleId == 1)
                {

                    var info = ifExist.Where(x => x.Date >= DateTime.Today.AddDays(-7) && x.Date <= DateTime.Today).GroupBy(x => x.Date).Select(x => new
                    {
                        DayOfWeek = x.Key.DayOfWeek.ToString(),
                        TotalPassengers = ifExist.Where(y => y.Date == x.Key).Sum(y => y.NumberOfPassengers),
                        TotalEarned = ifExist.Where(y => y.Date == x.Key).Sum(y => y.Charges)
                    }).ToList();

                    return Ok(info);
                }
                else if (schecduleId == 2)
                {
                    var info = ifExist.Where(x => x.Date.Year == DateTime.Now.Year).GroupBy(x => x.Date.Month).Select(x => new
                    {
                        Month = new DateTime(2010, x.Key, 1).ToString("MMMM"),
                        TotalPassengers = ifExist.Where(y => y.Date.Month == x.Key).Sum(y => y.NumberOfPassengers),
                        TotalEarned = ifExist.Where(y => y.Date.Month == x.Key).Sum(y => y.Charges)
                    }).ToList();

                    return Ok(info);
                }
                else if (schecduleId == 3)
                {
                    var info = ifExist.GroupBy(x => x.Date.Year).Select(x => new
                    {
                        Year = x.Key.ToString(),
                        TotalPassengers = ifExist.Where(y => y.Date.Year == x.Key).Sum(y => y.NumberOfPassengers),
                        TotalEarned = ifExist.Where(y => y.Date.Year == x.Key).Sum(y => y.Charges)
                    }).ToList();

                    return Ok(info);
                }

                return BadRequest();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest();
            }
        }
    }
}
