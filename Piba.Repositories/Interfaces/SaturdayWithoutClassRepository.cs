namespace Piba.Repositories.Interfaces
{
    public interface SaturdayWithoutClassRepository
    {
        Task<bool> DateHasClassAsync(DateTime date);
    }
}
