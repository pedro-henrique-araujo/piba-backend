using Microsoft.AspNetCore.Mvc;

namespace Piba.Services.Interfaces
{
    public interface GoogleLoginService
    {
        Task<string> LoginAsync([FromHeader] string authorization);
    }
}