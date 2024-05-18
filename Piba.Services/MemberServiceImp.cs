using Piba.Data.Dto;
using Piba.Data.Enums;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class MemberServiceImp : MemberService
    {
        private readonly MemberRepository _memberRepository;
        private readonly SchoolAttendanceService _schoolAttendanceService;

        public MemberServiceImp(MemberRepository memberRepository, SchoolAttendanceService schoolAttendanceService)
        {
            _memberRepository = memberRepository;
            _schoolAttendanceService = schoolAttendanceService;
        }

        public async Task<List<MemberOptionDto>> GetOptionsAsync()
        {
            var options = await _memberRepository.GetAllInactiveAndActiveOptionsAsync();

            return options;
        }

        public async Task ReviewMembersActivityAsync()
        {
            await CheckStatusChangeFromActiveToInactiveAsync();
            await CheckStatusChangeFromInactiveToActiveAsync();
            await _memberRepository.SaveChangesAsync();
        }

        private async Task CheckStatusChangeFromActiveToInactiveAsync()
        {
            var members = await _memberRepository.GetAllActiveCreatedBefore21DaysAgoAsync();
            foreach (var member in members)
            {
                if (await _schoolAttendanceService.MemberIsPresentAtLeastOnceOnLastThreeClassesAsync(member.Id)) continue;

                member.Status = MemberStatus.Inactive;
                member.LastStatusUpdate = DateTime.UtcNow;
            }
        }

        private async Task CheckStatusChangeFromInactiveToActiveAsync()
        {
            var members = await _memberRepository.GetAllInactiveAsync();
            foreach (var member in members)
            {
                if (await _schoolAttendanceService.MemberMissedAnyOfLastThreeClassesAsync(member.Id)) continue;

                member.Status = MemberStatus.Active;
                member.LastStatusUpdate = DateTime.UtcNow;
            }
        }
    }
}