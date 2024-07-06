using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;

namespace Piba.Repositories.Tests
{
    public class StatusHistoryItemRepositoryImpTests : IDisposable
    {
        private readonly PibaDbContext _pibaDbContext;
        private readonly StatusHistoryItemRepositoryImp _statusHistoryItemRepository;
        private readonly DateTime _baseDate;

        public StatusHistoryItemRepositoryImpTests()
        {
            _pibaDbContext = Common.GenerateInMemoryDatabase(nameof(StatusHistoryItemRepositoryImpTests));
            _statusHistoryItemRepository = new StatusHistoryItemRepositoryImp(
                _pibaDbContext);

            var utcNow = DateTime.UtcNow;
            _baseDate = new DateTime(utcNow.Year, utcNow.Month, 1, 18, 0, 0);
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_ShouldCreateStatusHistoryItems()
        {
            var member = new Member { Name = "A", Status = MemberStatus.Active };
            var history = new StatusHistory { Month = _baseDate.Month, Year = _baseDate.Year };

            var items = new List<StatusHistoryItem>
            {
                new()
                {
                    Member = member,
                    StatusHistory = history
                },
                new()
                {
                    Member = member,
                    StatusHistory = history
                }
            };

            await _statusHistoryItemRepository.CreateAsync(items);

            var historyItems = _pibaDbContext.Set<StatusHistoryItem>().ToList();
            Assert.Equal(2, historyItems.Count());
        }

        [Fact]
        public async Task GetLastHistoryAsync_WhenCalled_ReturnCorrectData()
        {
            var member1 = new Member { Name = "B", Status = MemberStatus.Inactive };
            var member2 = new Member { Name = "A", Status = MemberStatus.Inactive };
            var lastMonth = _baseDate.AddMonths(-1);
            var lastYear = _baseDate.AddYears(-1).AddMonths(-1);

            var history = new StatusHistory { Month = lastMonth.Month, Year = lastMonth.Year };
            var history2 = new StatusHistory { Month = lastYear.Month, Year = lastYear.Year };
            var history3 = new StatusHistory { Month = _baseDate.Month, Year = _baseDate.Year };

            var items = new List<StatusHistoryItem>
            {
                new()
                {
                    Member = member1,
                    Status = MemberStatus.Active,
                    StatusHistory = history
                },
                new()
                {
                    Member = member2,
                    Status = MemberStatus.Inactive,
                    StatusHistory = history
                },
                new()
                {
                    Member = member1,
                    Status = MemberStatus.Active,
                    StatusHistory = history2
                },
                new()
                {
                    Member = member1,
                    Status = MemberStatus.Active,
                    StatusHistory = history3
                }
            };

            var schoolAttendance = new List<SchoolAttendance>
            {
                new() { Member = member1, CreatedDate = _baseDate.AddMonths(-1) },
                new() { Member = member1, CreatedDate = _baseDate.AddMonths(-1) },
                new() { Member = member2, CreatedDate = _baseDate.AddMonths(-1) }
            };

            await _pibaDbContext.AddRangeAsync(items);
            await _pibaDbContext.AddRangeAsync(schoolAttendance);

            await _pibaDbContext.SaveChangesAsync();

            var filter = new ValidTimeFilter
            {
               MinValidTime = TimeSpan.FromHours(0),
               MaxValidTime = TimeSpan.FromHours(0)
            };

            var result = await _statusHistoryItemRepository.GetLastHistoryAsync(filter);
            Assert.Equal(2, result.Count());
            Assert.Equal("A", result.First().Name);
            Assert.Equal(MemberStatus.Inactive, result.First().Status);
            Assert.Equal(1, result.First().Count);
            Assert.Equal("B", result.Last().Name);
            Assert.Equal(MemberStatus.Active, result.Last().Status);
            Assert.Equal(2, result.Last().Count);
        }

        public void Dispose()
        {
            _pibaDbContext.Database.EnsureDeleted();
            _pibaDbContext.Dispose();
        }
    }
}
