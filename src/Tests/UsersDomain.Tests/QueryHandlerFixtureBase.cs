using Moq;
using UsersApplication.Interfaces;
using UsersDomain.Models;
using UsersDomain.ValueObjects;

namespace UsersApplication.Tests;

public class QueryHandlerFixtureBase
{
    public Mock<IUnitOfWork> UnitOfWorkMock { get; }

    protected QueryHandlerFixtureBase()
    {
        UnitOfWorkMock = new Mock<IUnitOfWork>();
    }

    public User SetupExistingUser() =>
    new()
    {
        Id = UserId.Of(Guid.NewGuid()),
        FirstName = "TestFirstName",
        LastName = "TestLastName",
        Email = "test.email@gmail.com",
        Password = "Po=PF]PC6t.?8?ks)A6W"
    };

    public List<User> SetupExistingUsersList() =>
    [
        new() { Id = UserId.Of(Guid.NewGuid()), FirstName = "DTestFirstName", LastName = "CTestLastName" },
        new() { Id = UserId.Of(Guid.NewGuid()), FirstName = "ATestFirstName", LastName = "CTestLastName" },
        new() { Id = UserId.Of(Guid.NewGuid()), FirstName = "BTestFirstName", LastName = "ATestLastName" },
        new() { Id = UserId.Of(Guid.NewGuid()), FirstName = "ATestFirstName", LastName = "BTestLastName" },
        new() { Id = UserId.Of(Guid.NewGuid()), FirstName = "ETestFirstName", LastName = "DTestLastName" },
    ];
}