using Piba.Repositories.Interfaces;
using Piba.Data.Dto;
using Microsoft.EntityFrameworkCore;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class MemberServiceImp : MemberService
    {
        private MemberRepository _memberRepository;

        public MemberServiceImp(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<List<MemberOptionDto>> GetOptionsAsync()
        {
            var options = await _memberRepository.GetAllInactiveAndActiveQueryable()
                .Select(m => new MemberOptionDto { Id = m.Id, Name = m.Name })
                .ToListAsync();

            return options;
        }
    }
}