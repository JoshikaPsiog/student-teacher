using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using student.Repositories;

namespace student.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public StudentController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                    role = user.Role,  // ✅ Use Role
                    gender = user.Gender
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user", error = ex.Message });
            }
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
                    role = u.Role,  // ✅ Use Role
                    gender = u.Gender
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving users", error = ex.Message });
            }
        }
    }
}
