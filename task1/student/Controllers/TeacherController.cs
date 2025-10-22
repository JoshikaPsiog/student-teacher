using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using student.Models;
using student.Repositories;

namespace student.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public TeacherController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                return Ok(users.Select(u => new
                {
                    id = u.Id,
                    name = u.Name,
                    email = u.Email,
                    dob = u.Dob,
                    role = u.Role,
                    gender = u.Gender,
                    address = u.Address,
                    phonenumber = u.Phonenumber,
                    stage = u.Stage,
                    age = u.Age
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving users", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email,
                    dob = user.Dob,
                    role = user.Role,
                    gender = user.Gender,
                    address = user.Address,
                    phonenumber = user.Phonenumber,
                    stage = user.Stage,
                    age = user.Age
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Passwordhash);
                user.Passwordhash = hashedPassword;
                user.Createdat = DateTime.Now;
                user.Updatedat = DateTime.Now;
                user.Age = CalculateAge(user.Dob ?? DateTime.Now);

                await _userRepository.AddUser(user);
                return Ok(new { message = "User created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating user", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                var existingUser = await _userRepository.GetUserById(id);
                if (existingUser == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Dob = user.Dob;
                existingUser.Role = user.Role;
                existingUser.Gender = user.Gender;
                existingUser.Address = user.Address;
                existingUser.Phonenumber = user.Phonenumber;
                existingUser.Stage = user.Stage;
                existingUser.Updatedat = DateTime.Now;
                existingUser.Age = CalculateAge(user.Dob ?? DateTime.Now);

                if (!string.IsNullOrEmpty(user.Passwordhash))
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Passwordhash);
                    existingUser.Passwordhash = hashedPassword;
                }

                await _userRepository.UpdateUser(existingUser);
                return Ok(new { message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating user", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                await _userRepository.DeleteUser(id);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting user", error = ex.Message });
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
