using Moq;
using UsersApplication.Dtos;
using UsersApplication.Interfaces;
using UsersDomain.Models;
using UsersDomain.ValueObjects;

namespace UsersApplication.Tests;

public class CommandHandlerFixtureBase
{
    public Mock<IUnitOfWork> UnitOfWorkMock { get; }

    protected CommandHandlerFixtureBase()
    {
        UnitOfWorkMock = new Mock<IUnitOfWork>();
    }

    public CommandHandlerFixtureBase UnitOfWorkMock_Setup_SaveAsync_Verifiable()
    {
        UnitOfWorkMock
            .Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>()))
            .Verifiable();

        return this;
    }

    public CommandHandlerFixtureBase UnitOfWorkMock_Verify_SaveAsync()
    {
        UnitOfWorkMock
            .Verify(uow => uow.SaveAsync(default), Times.Once);

        return this;
    }

    public CommandHandlerFixtureBase UnitOfWorkMock_Verify_No_SaveAsync()
    {
        UnitOfWorkMock
            .Verify(uow => uow.SaveAsync(default), Times.Never);

        return this;
    }

    public UserDto SetupUserDto() => 
    new()
    {
        FirstName = "TestFirstName",
        LastName = "TestLastName",
        Email = "test.email@gmail.com",
        Password = "testPassword111"
    };

    public UserDto SetupUserDtoWithId(
         UserId? id = null) =>
    new()
    {
        Id = id is null ? Guid.NewGuid() : id.Value,
        FirstName = "TestFirstName",
        LastName = "TestLastName",
        Email = "test.email@gmail.com",
        Password = "Po=PF]PC6t.?8?ks)A6W"
    };

    public User SetupExistingUser(
        UserId? id = null, 
        string? email = null, 
        string? password = null ) => 
    new()
    {
        Id = id ?? UserId.Of(Guid.NewGuid()),
        FirstName = "TestFirstName",
        LastName = "TestLastName",
        Email = email ?? "test.email@gmail.com",
        Password = password ?? "Po=PF]PC6t.?8?ks)A6W",
    };
}