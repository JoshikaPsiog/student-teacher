using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StudentTeacherApp.Data;
using StudentTeacherApp.Controllers;
using StudentTeacherApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentTeacherApp.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            // Arrange: Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new UsersController(_context);
        }

        [Fact]
        public async Task GetUsers_ReturnsAllUsers()
        {
            // Arrange
            _context.Users.AddRange(
                new User { Name = "User1", Email = "user1@test.com", DateOfBirth = DateTime.Now.AddYears(-20), Designation = "Student" },
                new User { Name = "User2", Email = "user2@test.com", DateOfBirth = DateTime.Now.AddYears(-25), Designation = "Teacher" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task GetUser_ExistingId_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Name = "Test User",
                Email = "test@example.com",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Designation = "Student"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetUser(user.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetUser_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 9999;

            // Act
            var result = await _controller.GetUser(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_ValidUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var newUser = new User
            {
                Name = "New User",
                Email = "newuser@test.com",
                DateOfBirth = DateTime.Now.AddYears(-22),
                Designation = "Teacher"
            };

            // Act
            var result = await _controller.CreateUser(new CreateUserDto
            {
                Name = newUser.Name,
                Email = newUser.Email,
                DateOfBirth = newUser.DateOfBirth,
                Designation = newUser.Designation
            });

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.NotNull(createdResult.Value);

            // Verify user was added to database
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "newuser@test.com");
            Assert.NotNull(dbUser);
            Assert.Equal("New User", dbUser.Name);
        }

        [Fact]
        public async Task UpdateUser_ExistingUser_ReturnsNoContent()
        {
            // Arrange
            var user = new User
            {
                Name = "Original Name",
                Email = "update@test.com",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Designation = "Student"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var updateDto = new CreateUserDto
            {
                Name = "Updated Name",
                Email = "update@test.com",
                DateOfBirth = user.DateOfBirth,
                Designation = "Teacher"
            };

            // Act
            var result = await _controller.UpdateUser(user.Id, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify changes
            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.Equal("Updated Name", updatedUser.Name);
            Assert.Equal("Teacher", updatedUser.Designation);
        }

        [Fact]
        public async Task DeleteUser_ExistingUser_ReturnsNoContent()
        {
            // Arrange
            var user = new User
            {
                Name = "Delete User",
                Email = "delete@test.com",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Designation = "Student"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteUser(user.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify deletion
            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task DeleteUser_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 9999;

            // Act
            var result = await _controller.DeleteUser(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
