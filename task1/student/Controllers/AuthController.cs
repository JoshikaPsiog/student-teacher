using Microsoft.AspNetCore.Mvc;
using student.DTOs;
using student.Models;
using student.Repositories;
using student.Services;

namespace student.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmail(dto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Email already registered" });
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    Name = dto.Name,
                    Dob = dto.Dob,
                    Role = dto.Role,
                    Email = dto.Email,
                    Passwordhash = hashedPassword,
                    Gender = dto.Gender,
                    Address = dto.Address,
                    Phonenumber = dto.Phonenumber,
                    Stage = dto.Stage,
                    Createdat = DateTime.Now,
                    Updatedat = DateTime.Now,
                    Lastlogged = null,
                    Age = CalculateAge(dto.Dob)
                };

                await _userRepository.AddUser(user);

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Registration failed", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(dto.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Passwordhash);

                if (!isValid)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                // ✅ Line 81: Convert DateTime to string for NVARCHAR column
                user.Lastlogged = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                await _userRepository.UpdateUser(user);

                var token = _jwtService.GenerateToken(user);

                return Ok(new { token, message = "Login successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Login failed", error = ex.Message });
            }
        }

        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
