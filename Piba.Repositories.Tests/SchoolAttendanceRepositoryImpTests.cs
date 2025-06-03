using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;

namespace Piba.Repositories.Tests
{
    public class SchoolAttendanceRepositoryImpTests : IDisposable
    {
        private readonly PibaDbContext _pibaDbContext;
        private readonly SchoolAttendanceRepositoryImp _statusAttendanceRepository;
        private readonly DateTime _baseDate;

        public SchoolAttendanceRepositoryImpTests()
        {
            _pibaDbContext = Common.GenerateInMemoryDatabase(nameof(SchoolAttendanceRepositoryImpTests));
            _statusAttendanceRepository = new SchoolAttendanceRepositoryImp(_pibaDbContext);
            var utcDate = DateTime.UtcNow;
            _baseDate = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, 18, 0, 0);
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_ShouldCreateSchoolAttendance()
        {
            var schoolAttendance = new SchoolAttendance
            {
                Member = new()
                {
                    Name = "A",
                    Status = MemberStatus.Active,
                    LastStatusUpdate = _baseDate
                },
                CreatedDate = _baseDate,
                IsPresent = true
            };

            await _statusAttendanceRepository.CreateAsync(schoolAttendance);

            var result = await _pibaDbContext.Set<SchoolAttendance>().FirstOrDefaultAsync();
            Assert.NotNull(result);
            Assert.Equal(schoolAttendance.MemberId, result.MemberId);
            Assert.Equal(schoolAttendance.CreatedDate, result.CreatedDate);
            Assert.Equal(schoolAttendance.IsPresent, result.IsPresent);
        }

        [Fact]
        public async Task GetByDatesAsync_WhenCalled_ShouldReturnSchoolAttendanceCount()
        {
            var filter = await SetupDatabaseToTestGetByDatesAsync();

            var result = await _statusAttendanceRepository.GetByDatesAsync(filter);
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetLastMonthExcusesAsync_WhenCalled_ShouldReturnSchoolAttendances()
        {
            var member = new Member()
            {
                Name = "A",
                Status = MemberStatus.Active,
                LastStatusUpdate = _baseDate
            };


            var schoolAttendances = new List<SchoolAttendance>
            {
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddYears(-1).AddMonths(-1),
                    IsPresent = false
                },
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddMonths(-1),
                    IsPresent = false
                },
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddMonths(-1),
                    IsPresent = false
                },
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddMonths(-2),
                    IsPresent = false
                }
            };

            await _pibaDbContext.Set<SchoolAttendance>().AddRangeAsync(schoolAttendances);
            await _pibaDbContext.SaveChangesAsync();
            _pibaDbContext.ChangeTracker.Clear();

            var result = await _statusAttendanceRepository.GetLastMonthExcusesAsync();
            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(result, a => a.Member is null);
        }

        [Theory]
        [InlineData(MemberStatus.Active, false, 0)]
        [InlineData(MemberStatus.Inactive, false, 0)]
        [InlineData(MemberStatus.AlwaysExcused, false, 0)]
        [InlineData(MemberStatus.AlwaysExcused, true, 61)]
        public async Task GetAttendancesReportAsync_WhenCalled_ShouldReturnAttendancesReport(MemberStatus status, bool isPresent, int minutes)
        {
            var date = _baseDate.AddHours(1);
            var member = new Member()
            {
                Name = "A",
                Status = MemberStatus.Active,
                LastStatusUpdate = date
            };

            var member2 = new Member()
            {
                Name = "B",
                Status = status,
                LastStatusUpdate = date
            };

            var member3 = new Member()
            {
                Name = "C",
                Status = MemberStatus.Removed,
                LastStatusUpdate = date
            };

            var member4 = new Member()
            {
                Name = "D",
                Status = MemberStatus.Active,
                LastStatusUpdate = date
            };

            var schoolAttendances = new List<SchoolAttendance>
            {
                new()
                {
                    Member = member,
                    CreatedDate = date.AddDays(-1),
                    IsPresent = true
                },
                new()
                {
                    Member = member2,
                    CreatedDate = date.AddHours(1),
                    IsPresent = true
                },
                new()
                {
                    Member = member3,
                    CreatedDate = date,
                    IsPresent = true
                },
                new()
                {
                    Member = member4,
                    CreatedDate = date.AddMinutes(minutes),
                    IsPresent = isPresent
                },
                new()
                {
                    Member = member4,
                    CreatedDate = date.AddDays(1),
                    IsPresent = true
                },
            };

            await _pibaDbContext.Set<SchoolAttendance>().AddRangeAsync(schoolAttendances);
            await _pibaDbContext.SaveChangesAsync();
            _pibaDbContext.ChangeTracker.Clear();

            var dates = new List<DateOnly> 
            {
                DateOnly.FromDateTime(date.AddDays(-1)),
                DateOnly.FromDateTime(date)
            };

            var result = await _statusAttendanceRepository.GetAttendancesReportAsync(dates, date.AddMinutes(60).TimeOfDay, -1);

            Assert.Equal(2, result.Count);

            Assert.Contains(result, r => r.Key == DateOnly.FromDateTime(_baseDate.AddDays(-1))
                && r.Value.Count == 1
                && r.Value[0].Name == "A"
                && r.Value[0].Time == TimeOnly.FromDateTime(_baseDate.AddDays(-1)));

            Assert.Contains(result, r => r.Key == DateOnly.FromDateTime(_baseDate.AddHours(1))
                && r.Value.Count == 1
                && r.Value[0].Name == "B"
                && r.Value[0].Time == TimeOnly.FromDateTime(_baseDate.AddHours(1)));

            Assert.DoesNotContain(result, r => r.Value.Any(v => v.Name == "C"));
            Assert.DoesNotContain(result, r => r.Value.Any(v => v.Name == "D"));
        }

        private async Task<MemberAttendancesByDatesFilter> SetupDatabaseToTestGetByDatesAsync()
        {
            var members = new List<Member>
            {
                new()
                {
                    Name = "A",
                    Status = MemberStatus.Active,
                    LastStatusUpdate = _baseDate
                },
                new()
                {
                    Name = "B",
                    Status = MemberStatus.Active,
                    LastStatusUpdate = _baseDate
                }
            };

            var schoolAttendances = new List<SchoolAttendance>()
            {
                new()
                {
                    Member = members[0],
                    CreatedDate = _baseDate.AddHours(-1),
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
                    Member = members[0],
                    CreatedDate = _baseDate.AddHours(1),
                    IsPresent = false
                },

                new()
                {
                    Member = members[0],
                    CreatedDate = _baseDate.AddHours(1),
                    IsPresent = false
                },

                new()
                {
                    Member = members[0],
                    CreatedDate = _baseDate.AddHours(-2),
                    IsPresent = true
                },

                new()
                {
                    Member = members[0],
                    CreatedDate = _baseDate.AddHours(2),
                    IsPresent = true
                },

                new()
                {
                    Member = members[0],
                    CreatedDate = _baseDate.AddDays(1),
                    IsPresent = true
                }
            };

            await _pibaDbContext.Set<SchoolAttendance>()
                .AddRangeAsync(schoolAttendances);
            await _pibaDbContext.SaveChangesAsync();

            var filter = new MemberAttendancesByDatesFilter
            {
                MemberId = members[0].Id,
                Dates = new() { _baseDate.Date },
                MinValidTime = _baseDate.AddHours(-1).TimeOfDay,
                MaxValidTime = _baseDate.AddHours(1).TimeOfDay
            };
            return filter;
        }

        public void Dispose()
        {
            _pibaDbContext.Database.EnsureDeleted();
            _pibaDbContext.Dispose();
        }
    }
}
