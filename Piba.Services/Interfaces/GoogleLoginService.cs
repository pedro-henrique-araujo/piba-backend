using Microsoft.AspNetCore.Mvc;

namespace Piba.Services.Interfaces
{
    public interface GoogleLoginService
    {
        Task<string> LoginAsync(string googleIdentityToken);
        Task<string> LoginOrCreateAsync(string authorization, string role);
    }
}