using AutoMapper;
using CarPoolingAPI.Data;
using CarPoolingAPI.Dto;
using CarPoolingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace CarPoolingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public DataContext context { get; }
        public IMapper mapper { get; }

        public VehicleController(DataContext context, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpGet("GetVehicles"), Authorize(Roles = "Driver")]
        public IActionResult GetVehiclesByDriverToken([FromHeader]string token)
        {
            var jwt = new JwtSecurityToken(token);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

            if (driver == null)
                return Unauthorized();

            var vehicles = mapper.Map<List<VehicleDto>>(context.Vehicles.Where(x => x.DriverId == driver.Id && x.IsActive).OrderBy(x => x.Name).ToList());
            
            if (vehicles != null)
            {
                return Ok(vehicles);
            }

            return BadRequest();
        }

        [HttpPost("Create"), Authorize(Roles = "Driver")]
        public IActionResult Create([FromHeader] string token, [FromBody] VehicleDto vehicle)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
                string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

                var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

                if (driver == null)
                    return Unauthorized();

                var newVehicle = new Vehicle
                {
                    Name = vehicle.Name,
                    PlatNo = vehicle.PlatNo,
                    Color = vehicle.Color,
                    Capacity = vehicle.Capacity,
                    DriverId = driver.Id
                };

                this.context.Add(newVehicle);
                this.context.SaveChanges();

                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest();
            }
        }

        [HttpPut("Edit"), Authorize(Roles = "Driver")]
        public IActionResult Edit([FromHeader] string token, [FromBody] VehicleDto vehicle)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
                string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

                var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

                if (driver == null && driver.Id != vehicle.Id)
                    return Unauthorized();

                var whicleVehicle = this.context.Vehicles.FirstOrDefault(x => x.Id == vehicle.Id);
                whicleVehicle.Name = vehicle.Name;
                whicleVehicle.PlatNo = vehicle.PlatNo;
                whicleVehicle.Color = vehicle.Color;
                whicleVehicle.Capacity = vehicle.Capacity;

                this.context.Update(whicleVehicle);
                this.context.SaveChanges();

                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest();
            }
        }

        [HttpPost("Delete"), Authorize(Roles = "Driver")]
        public IActionResult Delete([FromHeader] string token, [FromForm] int vehicleId)
        {
            try
            {
                var jwt = new JwtSecurityToken(token);
                string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
                string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

                var driver = context.Drivers.FirstOrDefault(x => x.Username == username);
                
                if (driver == null)
                    return Unauthorized();

                var vehicle = this.context.Vehicles.FirstOrDefault(x => x.Id == vehicleId);
                vehicle.IsActive = false;

                if (vehicle == null)
                    return BadRequest();

                this.context.Update(vehicle);
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
