using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using student.Controllers;
using student.Repositories;
using student.Services;
using student.DTOs;
using student.Models;
using System.Threading.Tasks;
using System;

namespace student.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<JwtService> _mockJwtService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockJwtService = new Mock<JwtService>(null);
            _controller = new AuthController(_mockRepo.Object, _mockJwtService.Object);
        }

        [Fact]
        public async Task Register_NewUser_ReturnsOk()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Test123",
                Dob = new DateTime(2000, 1, 1),
                Role = "Student",
                Gender = "Male"
            };

            _mockRepo.Setup(r => r.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _mockRepo.Setup(r => r.AddUser(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Register_ExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "existing@example.com",
                Password = "Test123",
                Dob = new DateTime(2000, 1, 1),
                Role = "Student",
                Gender = "Male"
            };

            var existingUser = new User { Id = 1, Email = "existing@example.com" };
            _mockRepo.Setup(r => r.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "Test123"
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Test123");
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Passwordhash = hashedPassword,
                Name = "Test User",
                Role = "Student"
            };

            _mockRepo.Setup(r => r.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockRepo.Setup(r => r.UpdateUser(It.IsAny<User>()))
                .Returns(Task.CompletedTask);
            _mockJwtService.Setup(j => j.GenerateToken(It.IsAny<User>()))
                .Returns("fake-jwt-token");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Login_InvalidEmail_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "invalid@example.com",
                Password = "Test123"
            };

            _mockRepo.Setup(r => r.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("CorrectPassword");
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Passwordhash = hashedPassword
            };

            _mockRepo.Setup(r => r.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
