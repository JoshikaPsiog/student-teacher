using codestudent.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace codestudent.Tests.ControllerTests
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task GetUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUsers();

            // Assert
            var okResult = result.Should().BeOfType<ActionResult<IEnumerable<User>>>().Subject;
            var okObjectResult = okResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var users = okObjectResult.Value as IEnumerable<object>;
            users.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetUser_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUser(1);

            // Assert
            var okResult = result.Should().BeOfType<ActionResult<User>>().Subject;
            okResult.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetUser_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUser(999);

            // Assert
            var actionResult = result.Should().BeOfType<ActionResult<User>>().Subject;
            actionResult.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateUser_WithValidData_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContext();
            var controller = new UsersController(context);

            var newUser = new User
            {
                Name = "New User",
                Email = "newuser@test.com",
                DateOfBirth = new DateTime(1995, 6, 20),
                Designation = "Student"
            };

            // Act
            var result = await controller.CreateUser(newUser);

            // Assert
            var actionResult = result.Should().BeOfType<ActionResult<User>>().Subject;
            actionResult.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task CreateUser_WithExistingEmail_ShouldReturnBadRequest()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var controller = new UsersController(context);

            var duplicateUser = new User
            {
                Name = "Duplicate User",
                Email = "teacher@test.com",
                DateOfBirth = new DateTime(1995, 6, 20),
                Designation = "Student"
            };

            // Act
            var result = await controller.CreateUser(duplicateUser);

            // Assert
            var actionResult = result.Should().BeOfType<ActionResult<User>>().Subject;
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateUser_WithValidData_ShouldReturnNoContent()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var controller = new UsersController(context);

            var updatedUser = new User
            {
                UserId = 1,
                Name = "Updated Teacher",
                Email = "updatedteacher@test.com",
                DateOfBirth = new DateTime(1985, 1, 1),
                Designation = "Teacher"
            };

            // Act
            var result = await controller.UpdateUser(1, updatedUser);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateUser_WithMismatchedId_ShouldReturnBadRequest()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var controller = new UsersController(context);

            var updatedUser = new User
            {
                UserId = 1,
                Name = "Updated Teacher",
                Email = "updatedteacher@test.com",
                DateOfBirth = new DateTime(1985, 1, 1),
                Designation = "Teacher"
            };

            // Act
            var result = await controller.UpdateUser(2, updatedUser); // Mismatched ID

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteUser_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var controller = new UsersController(context);

            // Act
            var result = await controller.DeleteUser(2);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            // Verify deletion
            var user = await context.Users.FindAsync(2);
            user.Should().BeNull();
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var context = TestDbContextFactory.CreateInMemoryDbContextWithData();
            var controller = new UsersController(context);

            // Act
            var result = await controller.DeleteUser(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
