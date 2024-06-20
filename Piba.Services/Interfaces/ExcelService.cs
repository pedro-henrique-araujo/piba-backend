
namespace Piba.Services.Interfaces
{
    public interface ExcelService
    {
        Task<byte[]> GenerateStatusHistoryAsync();
    }
}
