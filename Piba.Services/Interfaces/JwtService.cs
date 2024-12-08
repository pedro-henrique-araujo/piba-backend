using Microsoft.AspNetCore.Identity;
using Piba.Data.Entities;

namespace Piba.Services.Interfaces
{
    public interface JwtService
    {
        Task<string> GenerateUserTokenAsync(PibaUser user);
    }
}