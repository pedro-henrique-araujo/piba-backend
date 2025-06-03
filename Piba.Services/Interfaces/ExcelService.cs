
namespace Piba.Services.Interfaces
{
    public interface ExcelService
    {
        Task<byte[]> GenerateStatusHistoryAsync();

        Task<byte[]> GenerateAttendanceReportAsync(DateTime dateTime);
    }
}
