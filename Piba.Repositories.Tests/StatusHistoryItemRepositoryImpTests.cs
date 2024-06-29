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
            _statusHistoryItemRepository = new StatusHistoryItemRepositoryImp(_pibaDbContext);
            var utcNow = DateTime.UtcNow;
            _baseDate = new DateTime(utcNow.Year, utcNow.Month, 1, 18, 0, 0);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateStatusHistoryItems()
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
        public async Task GetLastHistoryAsync_ShouldReturnLastMonthStatusHistory()
        {
            var aMonthAgo = _baseDate.AddMonths(-1);
            var history = new StatusHistory { Month = aMonthAgo.Month, Year = aMonthAgo.Year };
            var members = new List<Member>()
            {
                new()
                {
                    Name = "A"
                },
                new()
                {
                    Name = "B"
                },
                new()
                {
                    Name = "C"
                }
            };
           
            var attendances = new List<SchoolAttendance>
            {
                new()
                {
                    Member = members[0],
                    CreatedDate = aMonthAgo,
                    IsPresent = true
                }, 
                new()
                {
                    Member = members[0],
                    CreatedDate = aMonthAgo,
                    IsPresent = true
                },
                new()
                {
                    Member = members[1],
                    CreatedDate = aMonthAgo.AddHours(-1),
                    IsPresent = false
                },
                new()
                {
                    Member = members[1],
                    CreatedDate = aMonthAgo.AddHours(-2),
                    IsPresent = true
                },
                new()
                {
                    Member = members[1],
                    CreatedDate = aMonthAgo.AddHours(2),
                    IsPresent = true
                },
                new()
                {
                    Member = members[1],
                    CreatedDate = _baseDate,
                    IsPresent = true
                },
                new()
                {
                    Member = members[1],
                    CreatedDate = aMonthAgo.AddYears(-1),
                    IsPresent = true
                }, 
                new()
                {
                    Member = members[2],
                    CreatedDate = aMonthAgo.AddHours(1),
                    IsPresent = true
                },
            };

            await _pibaDbContext.Set<SchoolAttendance>().AddRangeAsync(attendances);
            await _pibaDbContext.SaveChangesAsync();

            var items = new List<StatusHistoryItem>
            {
                new()
                {
                    Member = members[0],
                    StatusHistory = history,
                    Status = MemberStatus.Active
                },
                new()
                {
                    Member = members[1],
                    StatusHistory = history,
                    Status = MemberStatus.Inactive
                },
                new()
                {
                    Member = members[2],
                    StatusHistory = history,
                    Status = MemberStatus.Active
                },
            };
            
            await _statusHistoryItemRepository.CreateAsync(items);

            var filter = new ValidTimeFilter
            {
                MinValidTime = new TimeSpan(18, 0, 0),
                MaxValidTime = new TimeSpan(19, 0, 0)
            };

            var lastMonthHistory = await _statusHistoryItemRepository.GetLastHistoryAsync(filter);

            Assert.Equal(3, lastMonthHistory.Count);

            Assert.Equal("A", lastMonthHistory.First().Name);
            Assert.Equal(MemberStatus.Active, lastMonthHistory.First().Status);
            Assert.Equal(2, lastMonthHistory.First().Count);

            Assert.Equal("B", lastMonthHistory.Last().Name);
            Assert.Equal(MemberStatus.Inactive, lastMonthHistory.Last().Status);
            Assert.Equal(1, lastMonthHistory.Last().Count);

            Assert.Equal("C", lastMonthHistory[1].Name);
            Assert.Equal(MemberStatus.Active, lastMonthHistory[1].Status);
            Assert.Equal(1, lastMonthHistory[1].Count);
        }

        public void Dispose()
        {
            _pibaDbContext.Database.EnsureDeleted();
            _pibaDbContext.Dispose();
        }
    }
}
