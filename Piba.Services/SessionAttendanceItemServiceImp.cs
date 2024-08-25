using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class SessionAttendanceItemServiceImp : SessionAttendanceItemService
    {
        private readonly SessionAttendanceItemRepository _sessionAttendanceItemRepository;

        public SessionAttendanceItemServiceImp(SessionAttendanceItemRepository sessionAttendanceItemRepository)
        {
            _sessionAttendanceItemRepository = sessionAttendanceItemRepository;
        }

        public async Task CreateRangeAsync(Guid sessionAttendanceId, List<SessionAttendanceItemDto> items)
        {
            var itemsToInsert = items.Select(item => new SessionAttendanceItem
            {
                MemberId = item.MemberId,
                IsPresent = item.IsPresent,
                SessionAttendanceId = sessionAttendanceId,
            });

            await _sessionAttendanceItemRepository.CreateRangeAsync(itemsToInsert);
        }

        public async Task UpdateRangeAsync(Guid sessionAttendanceId, List<SessionAttendanceItemUpdateDto> items)
        {
            var itemsInDb = await _sessionAttendanceItemRepository.GetBySessionAttendanceIdAsync(sessionAttendanceId);
            foreach (var item in items)
            {
                var itemInDb = itemsInDb.Find(i => i.Id == item.Id);
                itemInDb.IsPresent = item.IsPresent;
            }

            await _sessionAttendanceItemRepository.UpdateRangeAsync(itemsInDb);
        }
    }
}
