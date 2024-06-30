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

            var result = await _statusAttendanceRepository.GetLastMonthExcusesAsync();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetLastMonthsSchoolAttendanceQueryable_WhenCalled_CorrectCount()
        {
            var member = new Member
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
                    CreatedDate = _baseDate.AddMonths(-1).AddHours(-1),
                    IsPresent = true,
                },
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddMonths(-1).AddHours(1),
                    IsPresent = true,
                }, 
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddMonths(-1).AddHours(-2),
                    IsPresent = false,
                },
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddMonths(-2),
                    IsPresent = true,
                },
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddYears(-1).AddMonths(-1),
                    IsPresent = true,
                },
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddMonths(-1).AddHours(-2).AddMinutes(59),
                    IsPresent = true,
                },
                new()
                {
                    Member = member,
                    CreatedDate = _baseDate.AddMonths(-1).AddHours(1).AddMinutes(1),
                    IsPresent = true,
                },
            };

            await _pibaDbContext.Set<SchoolAttendance>().AddRangeAsync(schoolAttendances);
            await _pibaDbContext.SaveChangesAsync();

            var filter = new ValidTimeFilter
            {
                MinValidTime = _baseDate.AddHours(-1).TimeOfDay,
                MaxValidTime = _baseDate.AddHours(1).TimeOfDay
            };

            var result = await _statusAttendanceRepository
                .GetLastMonthsSchoolAttendanceQueryable(filter)
                .ToListAsync();
            Assert.Equal(3, result.Count());
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
