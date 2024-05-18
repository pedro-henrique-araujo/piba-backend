
namespace Piba.Services.Interfaces
{
    public interface EnvironmentVariables
    {
        string DeveloperEmail { get; }
        string EmailHost { get; }
        string FromPassword { get; }
        string FromEmail { get; }
        TimeSpan MinValidTime { get; }
        TimeSpan MaxValidTime { get; }
    }
}