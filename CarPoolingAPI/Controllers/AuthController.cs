using CarPoolingAPI.Data;
using CarPoolingAPI.Dto;
using CarPoolingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarPoolingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public DataContext context { get; }
        public IConfiguration configuration { get; }

        public AuthController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpGet("ConnectToServer")]
        public IActionResult ConnectToServer()
        {
            try
            {
                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return BadRequest();
            }
        }

        [HttpPost("Register")]
        public IActionResult Resgister(UserRegister request)
        {
            try
            {
                if (request.IsDriver)
                {
                    var driver = new Driver
                    {
                        Username = request.FirstName.Trim() + " " + request.LastName.Trim(),
                        FirstName = request.FirstName.Trim(),
                        LastName = request.LastName.Trim(),
                        PhoneNo = request.PhoneNo,
                        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    };

                    context.Add(driver);
                    context.SaveChanges();

                    UserLogin userLogin = new UserLogin
                    {
                        Username = driver.Username,
                        IsDriver = true
                    };

                    string token = CreateToken(userLogin);
                    return Ok(token);
                }
                else
                {
                    var passenger = new Passenger
                    {
                        Username = request.FirstName.Trim() + " " + request.LastName.Trim(),
                        FirstName = request.FirstName.Trim(),
                        LastName = request.LastName.Trim(),
                        PhoneNo = request.PhoneNo,
                        Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
                    };

                    context.Add(passenger);
                    context.SaveChanges();

                    UserLogin userLogin = new UserLogin
                    {
                        Username = passenger.Username,
                        IsDriver = false
                    };

                    string token = CreateToken(userLogin);
                    return Ok(token);
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.InnerException.Message);
            }
        }

        [HttpPost("EditProfile")]
        public IActionResult EditProfile([FromHeader]string idToken, [FromBody]UserDto request)
        {
            try
            {
                var jwt = new JwtSecurityToken(idToken);
                string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
                string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

                if (role == "Driver")
                {
                    var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

                    if (driver == null)
                    {
                        return Unauthorized();
                    }

                    driver.Username = request.Username;
                    driver.FirstName = request.FirstName;
                    driver.LastName = request.LastName;
                    driver.PhoneNo = request.PhoneNo;
                    driver.Gender = request.Gender;
                    driver.DateOfBirth = request.DateOfBirth;
                    driver.ProfileImage = request.ProfileImage;
                    context.Update(driver);
                    context.SaveChanges();

                    UserLogin userLogin = new UserLogin
                    {
                        Username = driver.Username,
                        IsDriver = true
                    };

                    string token = CreateToken(userLogin);
                    return Ok(token);
                }
                else
                {
                    var passenger = context.Passengers.FirstOrDefault(x => x.Username == username);

                    if (passenger == null)
                    {
                        return Unauthorized();
                    }

                    passenger.Username = request.Username;
                    passenger.FirstName = request.FirstName;
                    passenger.LastName = request.LastName;
                    passenger.PhoneNo = request.PhoneNo;
                    passenger.Gender = request.Gender;
                    passenger.DateOfBirth = request.DateOfBirth;
                    passenger.ProfileImage = request.ProfileImage;
                    context.Update(passenger);
                    context.SaveChanges();

                    UserLogin userLogin = new UserLogin
                    {
                        Username = passenger.Username,
                        IsDriver = false
                    };

                    string token = CreateToken(userLogin);
                    return Ok(token);
                }
            }
            catch (Exception err)
            {
                return BadRequest(err.InnerException.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLogin request)
        {
            try
            {
                if ((!request.IsDriver && !context.Passengers.Any(x => x.Username == request.Username || x.PhoneNo == request.Username)) ||
                    (request.IsDriver && !context.Drivers.Any(x => x.Username == request.Username || x.PhoneNo == request.Username)))
                {
                    return NotFound();
                }

                if (request.IsDriver)
                {
                    var driver = context.Drivers.FirstOrDefault(x => x.Username == request.Username.Trim() || x.PhoneNo == request.Username.Trim());
                    request.Username = driver.Username;

                    if (BCrypt.Net.BCrypt.Verify(request.Password, driver.Password))
                    {
                        string token = CreateToken(request);
                        return Ok(token);
                    }
                }
                else
                {
                    var passenger = context.Passengers.FirstOrDefault(x => x.Username == request.Username.Trim() || x.PhoneNo == request.Username.Trim());
                    request.Username = passenger.Username;

                    if (BCrypt.Net.BCrypt.Verify(request.Password, passenger.Password))
                    {
                        string token = CreateToken(request);
                        return Ok(token);
                    }
                }

                return BadRequest("Wrong Password");

            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        private string CreateToken(UserLogin request)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, request.IsDriver ? "Driver" : "Passenger")
            };

            var values = new
            {
                TokenCreated = DateTime.Now,
                TokenExpires = DateTime.Now.AddDays(7)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(claims: claims, expires: values.TokenExpires, signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            if (request.IsDriver == true)
            {
                var driver = context.Drivers.FirstOrDefault(x => x.Username == request.Username.Trim() || x.PhoneNo == request.Username.Trim());

                driver.RememberToken = jwt;
                driver.TokenCreated = values.TokenCreated;
                driver.TokenExpires = values.TokenExpires;

                context.Drivers.Update(driver);
                context.SaveChanges();
            }
            else
            {
                var passenger = context.Passengers.FirstOrDefault(x => x.Username == request.Username.Trim() || x.PhoneNo == request.Username.Trim());
                passenger.RememberToken = jwt;
                passenger.TokenCreated = values.TokenCreated;
                passenger.TokenExpires = values.TokenExpires;

                context.Passengers.Update(passenger);
                context.SaveChanges();
            }

            return jwt;
        }

        [HttpGet("GetUserByToken")]
        public IActionResult GetUserByToken(string idToken)
        {
            var jwt = new JwtSecurityToken(idToken);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            UserDto userDto;
            if (role == "Driver")
            {
                var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

                if (driver == null)
                {
                    return Unauthorized();
                }

                userDto = new UserDto
                {
                    Username = driver.Username,
                    FirstName = driver.FirstName,
                    LastName = driver.LastName,
                    PhoneNo = driver.PhoneNo,
                    Gender = driver.Gender,
                    DateOfBirth = driver.DateOfBirth,
                    ProfileImage = driver.ProfileImage,
                };
            }
            else
            {
                var passenger = context.Passengers.FirstOrDefault(x => x.Username == username);

                if (passenger == null)
                {
                    return Unauthorized();
                }

                userDto = new UserDto
                {
                    Username = passenger.Username,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    PhoneNo = passenger.PhoneNo,
                    Gender = passenger.Gender,
                    DateOfBirth = passenger.DateOfBirth,
                    ProfileImage = passenger.ProfileImage,
                };
            }

            return Ok(userDto);
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromForm]string idToken)
        {
            var jwt = new JwtSecurityToken(idToken);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            UserLogin userLogin = new UserLogin
            {
                Username = username,
                IsDriver = role == "Driver" ? true : false
            };

            if (role == "Driver")
            {
                var driver = context.Drivers.FirstOrDefault(x => x.Username == username);

                if (driver != null)
                {
                    if (driver.RememberToken != idToken)
                    {
                        return Unauthorized("Invalid Token.");
                    }
                    else if (driver.TokenExpires < DateTime.Now)
                    {
                        return Unauthorized("Token expired.");
                    }

                    string token = CreateToken(userLogin);
                    return Ok(token);
                }

            }
            else if (role == "Passenger")
            {

                var passenger = context.Passengers.FirstOrDefault(x => x.Username == username);

                if (passenger != null)
                {
                    if (!passenger.RememberToken.Equals(idToken))
                    {
                        return Unauthorized("Invalid Token.");
                    }
                    else if (passenger.TokenExpires < DateTime.Now)
                    {
                        return Unauthorized("Token expired.");
                    }

                    string token = CreateToken(userLogin);
                    return Ok(token);
                }
            }

            return BadRequest("error occurred");
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromForm] string idToken, [FromForm] string oldPassword, [FromForm]string newPassword)
        {
            var jwt = new JwtSecurityToken(idToken);
            string username = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Name").Value).Value;
            string role = jwt.Claims.First(c => c.Type == configuration.GetSection("Claims:Role").Value).Value;

            if (role == "Driver")
            {
                var driver = context.Drivers.FirstOrDefault(x => x.Username == username || x.PhoneNo == username);

                if (BCrypt.Net.BCrypt.Verify(oldPassword, driver.Password))
                {
                    driver.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

                    context.Update(driver);
                    context.SaveChanges();

                    return Ok();
                }
            }
            else
            {
                var passenger = context.Passengers.FirstOrDefault(x => x.Username == username || x.PhoneNo == username);

                if (BCrypt.Net.BCrypt.Verify(oldPassword, passenger.Password))
                {
                    passenger.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

                    context.Update(passenger);
                    context.SaveChanges();

                    return Ok();
                }
            }

            return BadRequest("error occurred");
        }
    }
}
