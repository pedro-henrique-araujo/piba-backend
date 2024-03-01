namespace Piba.Services.Interfaces
{
    public interface SaturdayWithoutClassService
    {
        Task<List<DateTime>> GetLastThreeClassesDatesAsync();
    }
}
