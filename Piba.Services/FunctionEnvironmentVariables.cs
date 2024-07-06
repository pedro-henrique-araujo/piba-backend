using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class FunctionEnvironmentVariables : EnvironmentVariables
    {
        public string DeveloperEmail { get; }
        public string EmailHost { get; }
        public string FromEmail { get; }
        public string FromPassword { get; }

        public TimeSpan MinValidTime { get; }

        public TimeSpan MaxValidTime { get; }

        public FunctionEnvironmentVariables()
        {
            DeveloperEmail = GetVariableValue("DeveloperEmail");
            EmailHost = GetVariableValue("SmtpHost");
            FromEmail = GetVariableValue("FromEmail");
            FromPassword = GetVariableValue("FromEmailPassword");
            MinValidTime = TimeSpan.Parse(GetVariableValue("MinValidTime"));
            MaxValidTime = TimeSpan.Parse(GetVariableValue("MaxValidTime"));
        }

        private string GetVariableValue(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? string.Empty;
        }
    }
}
