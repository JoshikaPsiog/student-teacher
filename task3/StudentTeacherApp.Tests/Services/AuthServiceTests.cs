using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using StudentTeacherApp.Data;
using StudentTeacherApp.Services;
using StudentTeacherApp.DTOs;
using StudentTeacherApp.Models;
using Microsoft.Extensions.Configuration;

namespace StudentTeacherApp.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            // Arrange: Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Mock configuration for JWT key
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["JwtKey"]).Returns("ThisIsASecretKeyForJWTTokenGeneration12345");

            _authService = new AuthService(_context, _mockConfig.Object);
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsToken()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Test123",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Designation = "Student"
            };

            // Act
            var token = await _authService.Register(registerDto);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            // Verify user was added to database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            Assert.NotNull(user);
            Assert.Equal("Test User", user.Name);
            Assert.Equal("Student", user.Designation);
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsNull()
        {
            // Arrange
            var existingUser = new User
            {
                Name = "Existing User",
                Email = "duplicate@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pass123"),
                DateOfBirth = DateTime.Now.AddYears(-25),
                Designation = "Teacher"
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var registerDto = new RegisterDto
            {
                Name = "New User",
                Email = "duplicate@example.com",
                Password = "Test123",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Designation = "Student"
            };

            // Act
            var token = await _authService.Register(registerDto);

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var password = "Test123";
            var user = new User
            {
                Name = "Login User",
                Email = "login@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                DateOfBirth = DateTime.Now.AddYears(-20),
                Designation = "Teacher"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginDto
            {
                Email = "login@example.com",
                Password = password
            };

            // Act
            var token = await _authService.Login(loginDto);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public async Task Login_InvalidEmail_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "nonexistent@example.com",
                Password = "Test123"
            };

            // Act
            var token = await _authService.Login(loginDto);

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
                DateOfBirth = DateTime.Now.AddYears(-20),
                Designation = "Student"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            // Act
            var token = await _authService.Login(loginDto);

            // Assert
            Assert.Null(token);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
