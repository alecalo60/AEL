using AEL.Application.DTOs;

namespace AEL.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ResponseDTO> LoginAsync(LoginRequestDto request);
    }
}
