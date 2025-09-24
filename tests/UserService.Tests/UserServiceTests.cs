using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Tests;

public class UserServiceTests
{
    private UserDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new UserDbContext(options);
    }

    private IConfiguration GetConfiguration()
    {
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["Jwt:Key"]).Returns("ThisIsASecretKeyForTestingPurposes123!");
        mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        mockConfig.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
        return mockConfig.Object;
    }

    [Fact]
    public async Task RegisterAsync_ShouldAddUser_WhenNotExists()
    {
        var db = GetDbContext();
        var service = new UserService.Infrastructure.Services.UserService(db, GetConfiguration());

        var user = await service.RegisterAsync("testuser", "password123");

        user.Should().NotBeNull();
        user.Name.Should().Be("testuser");

        var dbUser = await db.Users.FirstOrDefaultAsync(u => u.Name == "testuser");
        dbUser.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenUserExists()
    {
        var db = GetDbContext();
        db.Users.Add(new User { Name = "testuser" });
        await db.SaveChangesAsync();

        var service = new UserService.Infrastructure.Services.UserService(db, GetConfiguration());

        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await service.RegisterAsync("testuser", "password123");
        });
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var db = GetDbContext();
        var service = new UserService.Infrastructure.Services.UserService(db, GetConfiguration());
        var user = await service.RegisterAsync("testuser", "password123");

        var result = await service.LoginAsync("testuser", "password123");

        result.Item1.Should().NotBeNull(); // JWT token
        result.userId.Should().Be(user.Id);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenCredentialsInvalid()
    {
        var db = GetDbContext();
        var service = new UserService.Infrastructure.Services.UserService(db, GetConfiguration());
        await service.RegisterAsync("testuser", "password123");

        var result = await service.LoginAsync("testuser", "wrongpassword");

        result.Item1.Should().BeNull();
        result.userId.Should().BeNull();
    }
}
