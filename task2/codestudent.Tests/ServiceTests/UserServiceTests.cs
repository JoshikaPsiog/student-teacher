using codestudent.Tests.Helpers;
using UserManagementAPI.Models;

namespace codestudent.Tests.ServiceTests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var users = await userService.GetAllUsersAsync();
            users.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidId_ShouldReturnUser()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var user = await userService.GetUserByIdAsync(1);
            user.Should().NotBeNull();
            user!.Name.Should().Be("Test Teacher");
        }

        [Fact]
        public async Task GetUserByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var user = await userService.GetUserByIdAsync(999);
            user.Should().BeNull();
        }

        [Fact]
        public async Task CreateUserAsync_WithPassword_ShouldCreateRegisteredUser()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContext();
            var userService = new UserService(context);

            var registerDto = new RegisterDto
            {
                Name = "New User",
                Email = "newuser@test.com",
                Password = "Password123",
                DateOfBirth = new DateTime(1995, 6, 20),
                Designation = "Student"
            };

            var result = await userService.CreateUserAsync(registerDto);
            result.Should().NotBeNull();
            result.Name.Should().Be("New User");
            result.IsRegistered.Should().BeTrue();
        }

        [Fact]
        public async Task CreateUserAsync_WithExistingEmail_ShouldThrowException()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);

            var registerDto = new RegisterDto
            {
                Name = "Duplicate User",
                Email = "teacher@test.com",
                Password = "Password123",
                DateOfBirth = new DateTime(1995, 6, 20),
                Designation = "Student"
            };

            Func<Task> act = async () => await userService.CreateUserAsync(registerDto);
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task UpdateUserAsync_WithValidData_ShouldUpdateUser()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);

            var updateDto = new RegisterDto
            {
                Name = "Updated Teacher",
                Email = "updatedteacher@test.com",
                DateOfBirth = new DateTime(1985, 1, 1),
                Designation = "Teacher",
                Password = ""
            };

            var result = await userService.UpdateUserAsync(1, updateDto);
            result.Should().NotBeNull();
            result!.Name.Should().Be("Updated Teacher");
        }

        [Fact]
        public async Task UpdateUserAsync_WithInvalidId_ShouldReturnNull()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);

            var updateDto = new RegisterDto
            {
                Name = "Non-existent User",
                Email = "nonexistent@test.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Designation = "Student",
                Password = ""
            };

            var result = await userService.UpdateUserAsync(999, updateDto);
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteUserAsync_WithValidId_ShouldReturnTrue()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var result = await userService.DeleteUserAsync(2);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_WithInvalidId_ShouldReturnFalse()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var result = await userService.DeleteUserAsync(999);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnUser()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var result = await userService.AuthenticateAsync("teacher@test.com", "Teacher123");
            result.Should().NotBeNull();
            result!.Email.Should().Be("teacher@test.com");
        }

        [Fact]
        public async Task AuthenticateAsync_WithInvalidPassword_ShouldReturnNull()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var result = await userService.AuthenticateAsync("teacher@test.com", "WrongPassword");
            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_WithNonExistentUser_ShouldReturnNull()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var result = await userService.AuthenticateAsync("nonexistent@test.com", "AnyPassword");
            result.Should().BeNull();
        }

        [Fact]
        public async Task UserExistsAsync_WithExistingEmail_ShouldReturnTrue()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var result = await userService.UserExistsAsync("teacher@test.com");
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UserExistsAsync_WithNonExistingEmail_ShouldReturnFalse()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var userService = new UserService(context);
            var result = await userService.UserExistsAsync("nonexistent@test.com");
            result.Should().BeFalse();
        }
    }
}
