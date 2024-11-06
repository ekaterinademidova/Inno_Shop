
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UsersApplication.Interfaces.Data;
using UsersApplication.Users.EventHandlers.Domain;
using UsersInfrastructure.Data;
using UsersInfrastructure.Tests.Fixtures;
using FluentAssertions;
using UsersApplication.Dtos;

namespace UsersInfrastructure.Tests 
{ 
    public sealed class UnitOfWorkTests
    {
        [Fact]
        public async Task Should_SaveAsync_Returns_True()
        {
            // Arrange
            var dbContextMock = new Mock<IApplicationDbContext>();
            dbContextMock
                .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var loggerMock = new Mock<ILogger<UnitOfWork>>();
            var unitOfWork = UnitOfWorkTestFixture.GetUnitOfWork(dbContextMock.Object, loggerMock.Object);

            // Act
            var result = await unitOfWork.SaveAsync(default);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_SaveAsync_Returns_False()
        {
            // Arrange
            var dbContextMock = new Mock<IApplicationDbContext>();
            var loggerMock = new Mock<ILogger<UnitOfWork>>();

            dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException("Error", new Exception("Exception message.")));

            var unitOfWork = UnitOfWorkTestFixture.GetUnitOfWork(dbContextMock.Object, loggerMock.Object);

            // Act
            var result = await unitOfWork.SaveAsync(default);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Saving_With_DbUpdateException()
        {
            // Arrange
            var dbContextMock = new Mock<IApplicationDbContext>();
            var loggerMock = new Mock<ILogger<UnitOfWork>>();

            dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Exception message."));

            var unitOfWork = UnitOfWorkTestFixture.GetUnitOfWork(dbContextMock.Object, loggerMock.Object);

            // Act
            Func<Task> action = async () => await unitOfWork.SaveAsync(default);

            // Assert
            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Exception message.");
        }
    }
}