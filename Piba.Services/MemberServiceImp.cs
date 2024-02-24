using Piba.Repositories.Interfaces;
using Piba.Data.Dto;
using Microsoft.EntityFrameworkCore;
using Piba.Services.Interfaces;
using Piba.Data.Enums;

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
            var members = await _memberRepository.GetAllActiveAsync();
            foreach (var member in members)
            {
                if (await _schoolAttendanceService.MemberIsPresentAtLeastOnceOnLastThreeSaturdaysAsync(member.Id)) continue;

                member.Status = MemberStatus.Inactive;
            }
        }

        private async Task CheckStatusChangeFromInactiveToActiveAsync()
        {
            var members = await _memberRepository.GetAllInactiveAsync();
            foreach(var member in members)
            {
                if (await _schoolAttendanceService.MemberMissedAnyOfLastThreeClassesAsync(member.Id)) continue;
                member.Status = MemberStatus.Active;
            }
        }
    }
}