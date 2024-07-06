using Moq;
using Piba.Data.Dto;
using Piba.Services.Interfaces;
using System.Net.Mail;

namespace Piba.Services.Tests
{
    public class EmailServiceImpTests
    {
        private readonly Mock<SmtpClientWrapper> _smtpPibaClientMock;
        private readonly Mock<EnvironmentVariables> _environmentVariablesMock;
        private readonly EmailServiceImp _emailService;

        public EmailServiceImpTests()
        {
            _smtpPibaClientMock = new Mock<SmtpClientWrapper>();
            _environmentVariablesMock = new Mock<EnvironmentVariables>();
            _emailService = new EmailServiceImp(
                _smtpPibaClientMock.Object, 
                _environmentVariablesMock.Object);
        }

        [Fact]
        public void SendEmailToDeveloper_WhenCalled_SendCorrectEmail()
        {
            var sendEmailDto = new SendEmailDto
            {
                Subject = "abc",
                Body = "def"
            };

            _environmentVariablesMock.Setup(m => m.FromEmail).Returns("from@test");
            _environmentVariablesMock.Setup(m => m.DeveloperEmail).Returns("to@test");

            _emailService.SendEmailToDeveloper(sendEmailDto);

            _smtpPibaClientMock.Verify(c =>
                    c.Send(It.Is<MailMessage>(m =>
                        m.From.Address == "from@test" &&
                        m.To.First().Address == "to@test" &&
                        m.Subject == sendEmailDto.Subject &&
                        m.Body == sendEmailDto.Body)),
                    Times.Once);
        }
    }
}
