using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CommanResult;
using Ecommerce.Shared.DTOS.IdentityDTOS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) 
                return Error.invalidCredentials("Invalid Email or Password");
                    
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);   
            if (!isPasswordValid)
                return Error.invalidCredentials("Password Invalid");
            return new UserDTO(user.Email, user.DisplayName, "Token");
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User= new ApplicationUser()
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber
            };
            var IdentityResult=await _userManager.CreateAsync(User, registerDTO.Password);

            if(IdentityResult.Succeeded)
                return Result<UserDTO>.Ok(new UserDTO(User.Email, User.DisplayName, "Token"));

            var errors = IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();
            return Result<UserDTO>.Fail(errors);
        }
    }
}
