using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using student.Controllers;
using student.Repositories;
using student.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace student.Tests.Controllers
{
    public class TeacherControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly TeacherController _controller;

        public TeacherControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _controller = new TeacherController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkWithUserList()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "User 1", Email = "user1@test.com", Role = "Student" },
                new User { Id = 2, Name = "User 2", Email = "user2@test.com", Role = "Teacher" }
            };

            _mockRepo.Setup(r => r.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count());
        }

        [Fact]
        public async Task GetUser_ExistingId_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Role = "Student"
            };

            _mockRepo.Setup(r => r.GetUserById(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetUser_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUser(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_ValidUser_ReturnsOk()
        {
            // Arrange
            var newUser = new User
            {
                Name = "New User",
                Email = "new@example.com",
                Passwordhash = "password123",
                Role = "Student",
                Gender = "Male",
                Dob = new DateTime(2000, 1, 1)
            };

            _mockRepo.Setup(r => r.AddUser(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateUser(newUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ExistingUser_ReturnsOk()
        {
            // Arrange
            var existingUser = new User
            {
                Id = 1,
                Name = "Old Name",
                Email = "old@example.com",
                Role = "Student",
                Passwordhash = "hash"
            };

            var updatedUser = new User
            {
                Name = "New Name",
                Email = "new@example.com",
                Role = "Teacher"
            };

            _mockRepo.Setup(r => r.GetUserById(1)).ReturnsAsync(existingUser);
            _mockRepo.Setup(r => r.UpdateUser(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateUser(1, updatedUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ExistingUser_ReturnsOk()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User" };

            _mockRepo.Setup(r => r.GetUserById(1)).ReturnsAsync(user);
            _mockRepo.Setup(r => r.DeleteUser(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task DeleteUser_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.DeleteUser(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
