using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class SessionAttendanceServiceImp : SessionAttendanceService
    {
        private readonly SessionAttendanceRepository _sessionAttendanceRepository;
        private readonly SessionAttendanceItemService _sessionAttendanceItemService;

        public SessionAttendanceServiceImp(SessionAttendanceRepository sessionAttendanceRepository, SessionAttendanceItemService sessionAttendanceItemService)
        {
            _sessionAttendanceRepository = sessionAttendanceRepository;
            _sessionAttendanceItemService = sessionAttendanceItemService;
        }

        public async Task CreateAsync(SessionAttendanceDto sessionAttendanceDto)
        {
            var attendance = new SessionAttendance();
            attendance.DateTime = sessionAttendanceDto.DateTime ?? DateTime.Now;
            await _sessionAttendanceRepository.CreateAsync(attendance);
            await _sessionAttendanceItemService.CreateRangeAsync(attendance.Id, sessionAttendanceDto.Items);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _sessionAttendanceRepository.DeleteAsync(id);
        }

        public async Task<SessionAttendanceOutputDto> GetByIdAsync(Guid id)
        {
            var record = await _sessionAttendanceRepository.GetByIdAsync(id);
            var output = new SessionAttendanceOutputDto();
            output.Id = record.Id;
            output.DateTime = record.DateTime;
            output.Items = record.SessionAttendanceItems.Select(item => new SessionAttendanceItemOutputDto()
            {
                Id = item.Id,
                MemberName = item.Member.Name,
                IsPresent = item.IsPresent
            });
            return output;
        }

        public async Task<RecordsPage<SessionAttendance>> PaginateAsync(PaginationQueryParameters paginationQueryParameters)
        {
            var page = new RecordsPage<SessionAttendance>();
            page.Records = await _sessionAttendanceRepository.PaginateAsync(paginationQueryParameters);
            page.Total = await _sessionAttendanceRepository.GetTotalAsync();
            return page;
        }

        public async Task UpdateAsync(SessionAttendanceUpdateDto sessionAttendanceUpdateDto)
        {
            if (sessionAttendanceUpdateDto.DateTime is not null)
            {
                var attendance = await _sessionAttendanceRepository.GetByIdAsync(sessionAttendanceUpdateDto.Id);

                attendance.DateTime = sessionAttendanceUpdateDto.DateTime.Value;
                await _sessionAttendanceRepository.UpdateAsync(attendance);
            }
            await _sessionAttendanceItemService.UpdateRangeAsync(sessionAttendanceUpdateDto.Id, sessionAttendanceUpdateDto.Items);
        }
    }
}
