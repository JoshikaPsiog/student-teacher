using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(MapToUserResponse);
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? MapToUserResponse(user) : null;
        }

        public async Task<UserResponse> CreateUserAsync(RegisterDto registerDto)
        {
            if (await UserExistsAsync(registerDto.Email))
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var user = new User
            {
                Name = registerDto.Name,
                DateOfBirth = registerDto.DateOfBirth,
                Designation = registerDto.Designation,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                IsRegistered = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToUserResponse(user);
        }

        public async Task<UserResponse> UpdateUserAsync(int id, RegisterDto registerDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            if (user.Email != registerDto.Email && await UserExistsAsync(registerDto.Email))
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            user.Name = registerDto.Name;
            user.DateOfBirth = registerDto.DateOfBirth;
            user.Designation = registerDto.Designation;
            user.Email = registerDto.Email;

            if (!string.IsNullOrEmpty(registerDto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return MapToUserResponse(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || string.IsNullOrEmpty(user.Password))
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        private UserResponse MapToUserResponse(User user)
        {
            return new UserResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                DateOfBirth = user.DateOfBirth,
                Designation = user.Designation,
                Email = user.Email,
                IsRegistered = user.IsRegistered
            };
        }
    }

}
