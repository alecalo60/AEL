using AEL.Application.DTOs;
using AEL.Application.Interfaces.Services;
using AEL.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AEL.Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IRoleService _roleService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService,
            IRoleService roleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _roleService = roleService;
        }



        public async Task<ResponseDTO> LoginAsync(LoginRequestDto request)
        {
            var response = new ResponseDTO();

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            // Validate user credentials
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = "Invalid credentials.";
                return response;
            }

            // Generate the JWT token
            var token = await _jwtService.GenerateToken(user, _userManager);

            response.Success = true;
            response.Message = "Login successful.";
            response.Data = token;

            return response;
        }
    }
}
