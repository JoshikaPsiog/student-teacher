using Microsoft.Extensions.Configuration;

namespace codestudent.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        public static ApplicationDbContext CreateInMemoryDbContextWithData()
        {
            var context = CreateInMemoryDbContext();

            context.Users.AddRange(
                new User
                {
                    UserId = 1,
                    Name = "Test Teacher",
                    Email = "teacher@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Teacher123"),
                    DateOfBirth = new DateTime(1985, 1, 1),
                    Designation = "Teacher",
                    IsRegistered = true
                },
                new User
                {
                    UserId = 2,
                    Name = "Test Student",
                    Email = "student@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Student123"),
                    DateOfBirth = new DateTime(2000, 5, 15),
                    Designation = "Student",
                    IsRegistered = true
                },
                new User
                {
                    UserId = 3,
                    Name = "Pending User",
                    Email = "pending@test.com",
                    Password = null,
                    DateOfBirth = new DateTime(2001, 3, 10),
                    Designation = "Student",
                    IsRegistered = false
                }
            );

            context.SaveChanges();
            return context;
        }

        public static IConfiguration GetMockConfiguration()
        {
            var configData = new Dictionary<string, string>
            {
                {"Jwt:Key", "SuperSecretKeyThatIsAtLeast32CharactersLongForTesting!"},
                {"Jwt:Issuer", "UserManagementAPI"},
                {"Jwt:Audience", "UserManagementClient"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(configData!)
                .Build();
        }
    }
}
