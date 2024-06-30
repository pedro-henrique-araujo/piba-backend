namespace Piba.Repositories.Interfaces
{
    public interface SaturdayWithoutClassRepository
    {
        Task<bool> DateWouldHaveClassAsync(DateTime date);
    }
}
