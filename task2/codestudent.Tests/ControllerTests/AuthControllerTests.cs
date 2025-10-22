using codestudent.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Controllers;

namespace codestudent.Tests.ControllerTests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Register_WithNewUser_ShouldReturnOk()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContext();
            var config = TestDbContextFactory.GetMockConfiguration();
            var controller = new AuthController(context, config);

            var registerRequest = new RegisterRequest
            {
                Name = "New User",
                Email = "newuser@test.com",
                Password = "Password123",
                DateOfBirth = new DateTime(2000, 1, 1),
                Designation = "Student"
            };

            var result = await controller.Register(registerRequest);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Register_WithExistingEmail_ShouldReturnBadRequest()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var config = TestDbContextFactory.GetMockConfiguration();
            var controller = new AuthController(context, config);

            var registerRequest = new RegisterRequest
            {
                Name = "Duplicate User",
                Email = "teacher@test.com",
                Password = "Password123",
                DateOfBirth = new DateTime(2000, 1, 1),
                Designation = "Student"
            };

            var result = await controller.Register(registerRequest);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_CompletingPendingRegistration_ShouldReturnOk()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var config = TestDbContextFactory.GetMockConfiguration();
            var controller = new AuthController(context, config);

            var registerRequest = new RegisterRequest
            {
                Name = "Pending User",
                Email = "pending@test.com",
                Password = "NewPassword123",
                DateOfBirth = new DateTime(2001, 3, 10),
                Designation = "Student"
            };

            var result = await controller.Register(registerRequest);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnOkWithToken()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var config = TestDbContextFactory.GetMockConfiguration();
            var controller = new AuthController(context, config);

            var loginRequest = new LoginRequest
            {
                Email = "teacher@test.com",
                Password = "Teacher123"
            };

            var result = await controller.Login(loginRequest);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Login_WithInvalidEmail_ShouldReturnUnauthorized()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var config = TestDbContextFactory.GetMockConfiguration();
            var controller = new AuthController(context, config);

            var loginRequest = new LoginRequest
            {
                Email = "nonexistent@test.com",
                Password = "Password123"
            };

            var result = await controller.Login(loginRequest);
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ShouldReturnUnauthorized()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var config = TestDbContextFactory.GetMockConfiguration();
            var controller = new AuthController(context, config);

            var loginRequest = new LoginRequest
            {
                Email = "teacher@test.com",
                Password = "WrongPassword"
            };

            var result = await controller.Login(loginRequest);
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Login_WithUnregisteredUser_ShouldReturnUnauthorized()
        {
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var config = TestDbContextFactory.GetMockConfiguration();
            var controller = new AuthController(context, config);

            var loginRequest = new LoginRequest
            {
                Email = "pending@test.com",
                Password = "AnyPassword"
            };

            var result = await controller.Login(loginRequest);
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }
    }
}
