namespace Piba.Repositories.Interfaces
{
    public interface LogRepository
    {
        Task LogMessageAsync(string message);
    }
}