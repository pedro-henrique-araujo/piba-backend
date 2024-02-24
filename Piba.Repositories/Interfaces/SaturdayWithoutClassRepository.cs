namespace Piba.Repositories.Interfaces
{
    public interface SaturdayWithoutClassRepository
    {
        Task<bool> AnyWithDateAsync(DateTime date);
    }
}
