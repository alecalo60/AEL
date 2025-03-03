using AEL.Application.Interfaces.Services;
using AEL.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AEL.Application.Services
{
    public class RoleService: IRoleService
    {
        private readonly UserManager<User> _userManager;

        public RoleService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

    }
}
