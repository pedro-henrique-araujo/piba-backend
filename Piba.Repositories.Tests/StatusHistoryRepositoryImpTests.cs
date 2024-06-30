using Microsoft.EntityFrameworkCore;
using Piba.Data;
using Piba.Data.Entities;

namespace Piba.Repositories.Tests
{
    public class StatusHistoryRepositoryImpTests : IDisposable
    {
        private readonly PibaDbContext _pibaDbContext;
        private readonly StatusHistoryRepositoryImp _statusHistoryRepository;
        private readonly DateTime _baseDate;

        public StatusHistoryRepositoryImpTests() 
        { 
            _pibaDbContext = Common.GenerateInMemoryDatabase(nameof(StatusHistoryRepositoryImpTests));
            _statusHistoryRepository = new StatusHistoryRepositoryImp(_pibaDbContext);
            _baseDate = DateTime.UtcNow;
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_StatusHistoryIsAdded()
        {
            var statusHistory = new StatusHistory { Year = 2021, Month = 1 };
            await _statusHistoryRepository.CreateAsync(statusHistory);

            var historySet = _pibaDbContext.Set<StatusHistory>();
            var historyList = await historySet.ToListAsync();

            Assert.Single(historyList);
            Assert.Equal(2021, historyList.First().Year);
            Assert.Equal(1, historyList.First().Month);
            Assert.False(historyList.First().IsSent);
        }

        [Fact]
        public async Task HistoryForLastMonthExistsAsync_WhenExists_AssertThatExists()
        {
            var lastMonth = _baseDate.AddMonths(-1);
            var statusHistory = new StatusHistory { Year = lastMonth.Year, Month = lastMonth.Month };
            await _statusHistoryRepository.CreateAsync(statusHistory);

            var exists = await _statusHistoryRepository.HistoryForLastMonthExistsAsync();

            Assert.True(exists);
        }

        [Fact]
        public async Task HistoryForLastMonthExistsAsync_WhenNotExists_AssertThatNotExists()
        {
            var statusHistory1 = new StatusHistory { Year = _baseDate.Year, Month = _baseDate.Month };
            await _statusHistoryRepository.CreateAsync(statusHistory1);
            var aYearAgo = _baseDate.AddYears(-1).AddMonths(-1);
            var statusHistory2 = new StatusHistory { Year = aYearAgo.Year, Month = aYearAgo.Month };
            await _statusHistoryRepository.CreateAsync(statusHistory2);

            var exists = await _statusHistoryRepository.HistoryForLastMonthExistsAsync();

            Assert.False(exists);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task IsHistoryOfLastMonthSentAsync_WhenHistoryExists_AssertWhetherIsSent(bool isSent)
        {
            var lastMonth = _baseDate.AddMonths(-1);
            var history = new StatusHistory { Year = lastMonth.Year, Month = lastMonth.Month, IsSent = isSent };
            await _statusHistoryRepository.CreateAsync(history);
            var actualIsSent = await _statusHistoryRepository.IsHistoryOfLastMonthSentAsync();
            Assert.Equal(isSent, actualIsSent);
        }

        [Fact]
        public async Task IsHistoryOfLastMonthSentAsync_WhenHistoryNotExists_AssertIsNotSent()
        {
            var result = await _statusHistoryRepository.IsHistoryOfLastMonthSentAsync();

            Assert.False(result);
        }

        [Fact]
        public async Task MarkLastMonthHistoryAsSentAsync_WhenHistoryExists_HistoryIsMarkedAsSent()
        {
            var lastMonth = _baseDate.AddMonths(-1);
            var history = new StatusHistory { Year = lastMonth.Year, Month = lastMonth.Month };
            await _statusHistoryRepository.CreateAsync(history);

            await _statusHistoryRepository.MarkLastMonthHistoryAsSentAsync();

            var historySet = _pibaDbContext.Set<StatusHistory>();
            var historyList = await historySet.ToListAsync();
            Assert.True(historyList.First().IsSent);
        }

        [Fact]
        public async Task MarkLastMonthHistoryAsSentAsync_WhenHistoryNotExists_NothingHappens()
        {
            await _statusHistoryRepository.MarkLastMonthHistoryAsSentAsync();

            var historySet = _pibaDbContext.Set<StatusHistory>();
            var historyList = await historySet.ToListAsync();
            Assert.Empty(historyList);
        }

        public void Dispose()
        {
            _pibaDbContext.Database.EnsureDeleted();
            _pibaDbContext.Dispose();
        }
    }
}
