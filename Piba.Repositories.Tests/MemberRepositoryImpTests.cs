using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;
using Piba.Data.Enums;

namespace Piba.Repositories.Tests
{
    public class MemberRepositoryImpTests : IDisposable
    {
        private readonly PibaDbContext _pibaDbContext;
        private readonly MemberRepositoryImp _memberRepository;

        public MemberRepositoryImpTests()
        {
            _pibaDbContext = Common.GenerateInMemoryDatabase(nameof(MemberRepositoryImpTests));
            _memberRepository = new MemberRepositoryImp(_pibaDbContext);
        }

        [Fact]
        public async Task GetAllInactiveAndActiveOptionsAsync_WhenCalled_ReturnsAllInactiveAndActiveOptions()
        {
            var member1 = new Member { Name = "A", Status = MemberStatus.Active };
            var member2 = new Member { Name = "B", Status = MemberStatus.Inactive };
            var member3 = new Member { Name = "C", Status = MemberStatus.AlwaysExcused };
            _pibaDbContext.AddRange(member1, member2, member3);
            await _pibaDbContext.SaveChangesAsync();

            var result = await _memberRepository.GetAllInactiveAndActiveOptionsAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Id == member1.Id && m.Name == member1.Name);
            Assert.Contains(result, m => m.Id == member2.Id && m.Name == member2.Name);
        }

        [Fact]
        public async Task GetAllActiveCreatedBefore21DaysAgoAsync_WhenCalled_ReturnsAllActiveMembersCreatedBefore21DaysAgo()
        {
            var member1 = new Member { Name = "A", Status = MemberStatus.Active, LastStatusUpdate = DateTime.Today.AddDays(-22) };
            var member2 = new Member { Name = "B", Status = MemberStatus.Active, LastStatusUpdate = DateTime.Today.AddDays(-23) };
            var member3 = new Member { Name = "C", Status = MemberStatus.Active, LastStatusUpdate = DateTime.Today.AddDays(-21) };
            var member4 = new Member { Name = "D", Status = MemberStatus.Inactive, LastStatusUpdate = DateTime.Today.AddDays(-21) };
            _pibaDbContext.AddRange(member1, member2, member3, member4);
            await _pibaDbContext.SaveChangesAsync();

            var result = await _memberRepository.GetAllActiveCreatedBefore21DaysAgoAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Name == member1.Name);
            Assert.Contains(result, m => m.Name == member2.Name);
        }

        [Fact]
        public async Task GetAllInactiveAsync_WhenCalled_ReturnsAllInactiveMembers()
        {
            var member1 = new Member { Name = "A", Status = MemberStatus.Inactive };
            var member2 = new Member { Name = "B", Status = MemberStatus.Active };
            var member3 = new Member { Name = "C", Status = MemberStatus.Inactive };
            await _pibaDbContext.Set<Member>()
                .AddRangeAsync(member1, member2, member3);

            await _pibaDbContext.SaveChangesAsync();

            var result = await _memberRepository.GetAllInactiveAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Name == member1.Name);
            Assert.Contains(result, m => m.Name == member3.Name);
        }

        [Fact]
        public async Task SaveChangesAsync_WhenCalled_SaveChanges()
        {
            var member = new Member { Name = "A", Status = MemberStatus.Active };

            _pibaDbContext.Entry(member).State = EntityState.Added;
            await _memberRepository.SaveChangesAsync();
            _pibaDbContext.Entry(member).State = EntityState.Detached;

            var result = await _pibaDbContext.Set<Member>().FirstOrDefaultAsync(m => m.Name == "A");

            Assert.Equal("A", result.Name);
        }

        [Fact]
        public async Task GetAllInactiveAndActiveAsync_WhenCalled_ReturnsAllInactiveAndActiveMembers()
        {
            var member1 = new Member { Name = "A", Status = MemberStatus.Active };
            var member2 = new Member { Name = "B", Status = MemberStatus.Inactive };
            var member3 = new Member { Name = "C", Status = MemberStatus.AlwaysExcused };
            var member4 = new Member { Name = "D", Status = MemberStatus.Removed };
            _pibaDbContext.AddRange(member1, member2, member3, member4);
            await _pibaDbContext.SaveChangesAsync();

            var result = await _memberRepository.GetAllInactiveAndActiveAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Name == member1.Name);
            Assert.Contains(result, m => m.Name == member2.Name);
        }

        [Fact]
        public async Task GetAllAlwaysExcusedAsync_WhenCalled_ReturnsAllAlwaysExcusedMembers()
        {
            var member1 = new Member { Name = "A", Status = MemberStatus.Active };
            var member2 = new Member { Name = "B", Status = MemberStatus.AlwaysExcused };
            var member3 = new Member { Name = "C", Status = MemberStatus.AlwaysExcused };
            var member4 = new Member { Name = "D", Status = MemberStatus.Inactive };
            var member5 = new Member { Name = "E", Status = MemberStatus.Removed };
            _pibaDbContext.AddRange(member1, member2, member3, member4);
            await _pibaDbContext.SaveChangesAsync();

            var result = await _memberRepository.GetAllAlwaysExcusedAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, m => m.Name == member2.Name);
            Assert.Contains(result, m => m.Name == member3.Name);
        }

        public void Dispose()
        {
            _pibaDbContext.Database.EnsureDeleted();
            _pibaDbContext.Dispose();
        }
    }
}
