using AEL.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AEL.Application.Interfaces.Services
{
    public interface IJwtService
    {
        Task<string> GenerateToken(User user, UserManager<User> userManager);
    }
}

