using AEL.Application.DTOs;
using AEL.Application.Interfaces.Services;
using AEL.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AEL.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        public UserService(UserManager<User> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }
    }
}
