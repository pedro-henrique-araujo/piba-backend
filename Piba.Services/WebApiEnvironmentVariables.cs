using Microsoft.Extensions.Configuration;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class WebApiEnvironmentVariables : EnvironmentVariables
    {
        public string DeveloperEmail { get; }
        public string EmailHost { get; }
        public string FromEmail { get; }
        public string FromPassword { get; }

        public TimeSpan MinValidTime { get; }

        public TimeSpan MaxValidTime { get; }

        public WebApiEnvironmentVariables(IConfiguration configuration)
        {
            DeveloperEmail = configuration["DeveloperEmail"];
            EmailHost = configuration["SmtpHost"];
            FromEmail = configuration["FromEmail"];
            FromPassword = configuration["FromEmailPassword"];
            MinValidTime = TimeSpan.Parse(configuration["MinValidTime"]);
            MaxValidTime = TimeSpan.Parse(configuration["MaxValidTime"]);
        }
    }
}
