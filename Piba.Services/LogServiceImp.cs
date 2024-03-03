using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piba.Services
{
    public class LogServiceImp : LogService
    {
        private readonly LogRepository _logRepository;

        public LogServiceImp(LogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task LogMessageAsync(string message)
        {
            await _logRepository.LogMessageAsync(message);
        }
    }
}
