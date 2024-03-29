﻿using Piba.Data.Dto;
using Piba.Data.Entities;

namespace Piba.Repositories.Interfaces
{
    public interface MemberRepository
    {
        Task<List<Member>> GetAllActiveAsync();
        Task<List<MemberOptionDto>> GetAllInactiveAndActiveOptionsAsync();
        Task<List<Member>> GetAllInactiveAsync();
        Task SaveChangesAsync();
    }
}
