using HomeHealthcare.API.Models;
using HomeHealthcare.API.Models.DTOs;

namespace HomeHealthcare.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
