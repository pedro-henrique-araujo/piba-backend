namespace Piba.Services.Interfaces
{
    public interface LogService
    {
        Task LogMessageAsync(string message);
    }
}