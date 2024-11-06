using Moq;
using UsersApplication.Interfaces;
using UsersApplication.ValueObjects;
using UsersDomain.Enums;
using UsersDomain.Models;
using UsersDomain.ValueObjects;
using UsersInfrastructure.Services;
using FluentAssertions;
using MimeKit;
using MailKit.Net.Smtp;
using UsersApplication.Interfaces.RepositoryContracts;
using UsersApplication.Interfaces.WrappersContracts;

namespace UsersInfrastructure.Tests
{
    public class EmailServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmailSettings> _emailSettingsMock;
        private readonly Mock<ISmtpClientWrapper> _smtpClientWrapperMock;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _emailSettingsMock = new Mock<IEmailSettings>();
            _emailSettingsMock.SetupGet(x => x.SmtpServer).Returns("smtp.example.com");
            _emailSettingsMock.SetupGet(x => x.SmtpPort).Returns(465);
            _emailSettingsMock.SetupGet(x => x.UseSsl).Returns(true);
            _emailSettingsMock.SetupGet(x => x.SmtpName).Returns("smtp_user");
            _emailSettingsMock.SetupGet(x => x.SenderName).Returns("Sender Name");
            _emailSettingsMock.SetupGet(x => x.SmtpPassword).Returns("smtp_password");
            _emailSettingsMock.SetupGet(x => x.SenderEmail).Returns("sender@example.com");

            _smtpClientWrapperMock = new Mock<ISmtpClientWrapper>();
            _smtpClientWrapperMock.Setup(client => client.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _smtpClientWrapperMock.Setup(client => client.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _smtpClientWrapperMock.Setup(client => client.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult("Success"));

            _emailService = new EmailService(_emailSettingsMock.Object, _unitOfWorkMock.Object, _smtpClientWrapperMock.Object);
        }

        [Fact]
        public async Task SendEmailConfirmationAsync_Should_Send_Email_Confirmation()
        {
            // Arrange
            var user = new User { Id = UserId.Of(Guid.NewGuid()), Email = "user@example.com" };

            _unitOfWorkMock
                .Setup(uow => uow.OperationToken.AddAsync(It.IsAny<OperationToken>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock
                .Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _emailService.SendEmailConfirmationAsync(user, default);

            // Assert
            await act.Should().NotThrowAsync();

            _unitOfWorkMock
                .Verify(uow => uow.OperationToken.AddAsync(It.IsAny<OperationToken>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock
                .Verify(uow => uow.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);

            _smtpClientWrapperMock
                .Verify(client => client.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
            _smtpClientWrapperMock
                .Verify(client => client.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _smtpClientWrapperMock
                .Verify(client => client.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>()), Times.Once);
            _smtpClientWrapperMock
                .Verify(client => client.DisconnectAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendEmailAsync_Should_Throw_Exception_When_Email_Sending_Fails()
        {
            // Arrange
            var message = new MimeMessage();

            // Настраиваем mock _smtpClientWrapperMock так, чтобы он выбросил исключение при попытке подключения
            _smtpClientWrapperMock
                .Setup(client => client.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act
            Func<Task> act = async () => await _emailService.SendEmailAsync(message, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"*Error sending email:*");

            _smtpClientWrapperMock
                .Verify(client => client.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
            _smtpClientWrapperMock
                .Verify(client => client.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _smtpClientWrapperMock
                .Verify(client => client.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>()), Times.Never);
            _smtpClientWrapperMock
                .Verify(client => client.DisconnectAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GenerateToken_Should_Return_Valid_Token()
        {
            // Arrange
            var userId = UserId.Of(Guid.NewGuid());
            var operationType = OperationType.EmailConfirmation;

            _unitOfWorkMock.Setup(uow => uow.OperationToken.AddAsync(It.IsAny<OperationToken>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var token = await _emailService.GenerateToken(userId, operationType, CancellationToken.None);

            // Assert
            token.Should().NotBeNullOrEmpty();
            _unitOfWorkMock.Verify(uow => uow.OperationToken.AddAsync(It.IsAny<OperationToken>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void GenerateLink_Should_Return_Correct_Url()
        {
            // Arrange
            var pathSegment = "confirm-email";
            var token = "sampleToken";

            // Act
            var link = EmailService.GenerateLink(pathSegment, token);

            // Assert
            link.Should().Be($"https://localhost:5051/users/{pathSegment}?token={token}");
        }

        [Fact]
        public void CreateMimeMessage_ShouldReturnCorrectMimeMessage()
        {
            // Arrange
            var user = new User { Email = "user@example.com" };
            var link = "https://example.com/confirm";
            var subject = "Email Confirmation";
            var description = "Please confirm your email";

            // Act
            var message = _emailService.CreateMimeMessage(user, link, subject, description);

            // Assert
            message.Should().NotBeNull();

            var from = message.From.Cast<MailboxAddress>().Single();
            from.Name.Should().Be(_emailSettingsMock.Object.SenderName);
            from.Address.Should().Be(_emailSettingsMock.Object.SenderEmail);

            var to = message.To.Cast<MailboxAddress>().Single();
            to.Name.Should().Be(user.FullName);
            to.Address.Should().Be(user.Email);

            message.Subject.Should().Be(subject);

            message.HtmlBody.Should().Contain(link);
        }
    }
}
