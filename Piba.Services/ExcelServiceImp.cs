﻿using Piba.Data.Dto;
using Piba.Data.Entities;
using Piba.Data.Enums;
using Piba.Repositories.Interfaces;
using Piba.Services.Interfaces;

namespace Piba.Services
{
    public class ExcelServiceImp : ExcelService
    {
        private readonly EnvironmentVariables _environmentVariables;
        private readonly StatusHistoryItemRepository _statusHistoryItemRepository;
        private readonly MemberRepository _memberRepository;
        private readonly SchoolAttendanceRepository _schoolAttendanceRepository;

        public ExcelServiceImp(
            EnvironmentVariables environmentVariables,
            StatusHistoryItemRepository statusHistoryItemRepository,
            MemberRepository memberRepository,
            SchoolAttendanceRepository schoolAttendanceRepository)
        {
            _environmentVariables = environmentVariables;
            _statusHistoryItemRepository = statusHistoryItemRepository;
            _memberRepository = memberRepository;
            _schoolAttendanceRepository = schoolAttendanceRepository;
        }

        public async Task<byte[]> GenerateStatusHistoryAsync()
        {
            var excelWrapper = new ExcelWrapper();
            await IncludeActiveAndInactiveAsync(excelWrapper);
            await IncludeAlwaysExcusedAsync(excelWrapper);
            await IncludeLastMonthsExcusesAsync(excelWrapper);
            return await excelWrapper.GetByteArrayAsync();
        }

        private async Task IncludeActiveAndInactiveAsync(ExcelWrapper excelWrapper)
        {
            var historyItems = await _statusHistoryItemRepository.GetLastHistoryAsync(new()
            {
                MinValidTime = _environmentVariables.MinValidTime,
                MaxValidTime = _environmentVariables.MaxValidTime,
            });


            excelWrapper.AddWorksheet<StatusHistoryReportDto>(new("Inativos")
            {
                Map = new()
                {
                    ["Nome"] = r => r.Name,
                    ["Número de presenças ou justificativas no mês passado"] = r => r.Count,
                },
                Rows = historyItems
                    .Where(i => i.Status == MemberStatus.Inactive)
                    .OrderBy(i => i.Name)
                    .ToList()
            });

            excelWrapper.AddWorksheet<StatusHistoryReportDto>(new("Ativos")
            {
                Map = new()
                {
                    ["Nome"] = r => r.Name,
                    ["Número de presenças ou justificativas no mês passado"] = r => r.Count,
                },
                Rows = historyItems
                    .Where(i => i.Status == MemberStatus.Active)
                    .OrderBy(i => i.Name)
                    .ToList()
            });
        }

        private async Task IncludeAlwaysExcusedAsync(ExcelWrapper excelWrapper)
        {
            var members = await _memberRepository.GetAllAlwaysExcusedAsync();
            excelWrapper.AddWorksheet<Member>(new("Sempre Justificados")
            {
                Map = new()
                {
                    ["Nome"] = m => m.Name,
                    ["Justificativa"] = m => m.RecurrentExcuse
                },
                Rows = members.OrderBy(m => m.Name).ToList()
            });
        }

        private async Task IncludeLastMonthsExcusesAsync(ExcelWrapper excelWrapper)
        {
            var lastMonthExcuses = await _schoolAttendanceRepository.GetLastMonthsExcusesAsync();
            excelWrapper.AddWorksheet<SchoolAttendance>(new("Justificativas")
            {
                Map = new()
                {
                    ["Nome"] = a => a.Member.Name,
                    ["Motivo"] = a => a.Excuse,
                    ["Data e Horário"] = a => a.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")

                },

                Rows = lastMonthExcuses.OrderBy(a => a.Member.Name).ToList()
            });
        }
    }
}
