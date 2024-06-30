//using Piba.Data.Dto;
//using Piba.Data.Entities;
//using Piba.Data.Enums;
//using Piba.Services.Interfaces;

//namespace Piba.Services
//{
//    public class StatusHistoryExcelManipulator
//    {
//        private readonly ExcelService _excelService;
//        private readonly EnvironmentVariables _environmentVariables;
//        private readonly ExcelWrapper _excelWrapper;

//        public StatusHistoryExcelManipulator(
//            ExcelService excelService,
//            EnvironmentVariables environmentVariables)
//        {
//            _excelService = excelService;
//            _environmentVariables = environmentVariables;
//            _excelWrapper = new ExcelWrapper();
//        }

//        public async Task<byte[]> GetByteArrayAsync()
//        {
//            return await _excelWrapper.GetByteArrayAsync();
//        }

//        public async Task IncludeActiveAndInactiveAsync()
//        {
//            var historyItems = await _excelService.GetLastHistoryAsync(new()
//            {
//                MinValidTime = _environmentVariables.MinValidTime,
//                MaxValidTime = _environmentVariables.MaxValidTime,
//            });


//            _excelWrapper.AddWorksheet<StatusHistoryReportDto>(new("Inativos")
//            {
//                Map = new()
//                {
//                    ["Nome"] = r => r.Name,
//                    ["Número de presenças ou justificativas no mês passado"] = r => r.Count,
//                },
//                Rows = historyItems
//                    .Where(i => i.Status == MemberStatus.Inactive)
//                    .OrderBy(i => i.Name)
//                    .ToList()
//            });

//            _excelWrapper.AddWorksheet<StatusHistoryReportDto>(new("Ativos")
//            {
//                Map = new()
//                {
//                    ["Nome"] = r => r.Name,
//                    ["Número de presenças ou justificativas no mês passado"] = r => r.Count,
//                },
//                Rows = historyItems
//                    .Where(i => i.Status == MemberStatus.Active)
//                    .OrderBy(i => i.Name)
//                    .ToList()
//            });
//        }

//        public async Task IncludeAlwaysExcusedAsync()
//        {
//            var members = await _excelService.GetAllAlwaysExcusedAsync();
//            _excelWrapper.AddWorksheet<Member>(new("Sempre Justificados")
//            {
//                Map = new()
//                {
//                    ["Nome"] = m => m.Name,
//                    ["Justificativa"] = m => m.RecurrentExcuse
//                },
//                Rows = members.OrderBy(m => m.Name).ToList()
//            });
//        }

//        public async Task IncludeLastMonthsExcusesAsync()
//        {
//            var lastMonthExcuses = await _excelService.GetLastMonthsExcusesAsync();
//            _excelWrapper.AddWorksheet<SchoolAttendance>(new("Justificativas")
//            {
//                Map = new()
//                {
//                    ["Nome"] = a => a.Member.Name,
//                    ["Motivo"] = a => a.Excuse,
//                    ["Data e Horário"] = a => a.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")

//                },

//                Rows = lastMonthExcuses.OrderBy(a => a.Member.Name).ToList()
//            });
//        }
//    }
//}
