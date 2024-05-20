using Piba.Data.Entities;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class MemberStatusHistoryServiceImp : MemberStatusHistoryService
    {
        private readonly MemberStatusHistoryRepository _memberStatusHistoryRepository;
        private readonly MemberRepository _memberRepository;

        public MemberStatusHistoryServiceImp(
            MemberStatusHistoryRepository memberStatusHistoryRepository, 
            MemberRepository memberRepository)
        {
            _memberStatusHistoryRepository = memberStatusHistoryRepository;
            _memberRepository = memberRepository;
        }

        public async Task CreateActivityHistoryForLastMonthIfItDoesNotExistAsync()
        {
            if (await _memberStatusHistoryRepository.HistoryForLastMonthExistsAsync()) return;

            var members = await _memberRepository.GetAllInactiveAndActiveAsync();

            var histories = members.Select(m => new MemberStatusHistory(m));

            await _memberStatusHistoryRepository.CreateAsync(histories);
        }

        public Task SendMemberStatusHistoryEmailToReceiversAsync()
        {
            throw new NotImplementedException();
        }
    }
}
