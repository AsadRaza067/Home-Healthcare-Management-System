using HomeHealthcare.API.Models;

namespace HomeHealthcare.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
