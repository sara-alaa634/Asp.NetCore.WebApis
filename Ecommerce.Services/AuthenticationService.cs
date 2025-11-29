using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CommanResult;
using Ecommerce.Shared.DTOS.IdentityDTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var User =await _userManager.FindByEmailAsync(email);
            if(User == null)
                return Error.NotFound("User Not Found");

            return new UserDTO(User.Email!, User.DisplayName, await CreateTokenAsync(User)); 
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) 
                return Error.invalidCredentials("Invalid Email or Password");
                    
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);   
            if (!isPasswordValid)
                return Error.invalidCredentials("Password Invalid");

            var Token = await CreateTokenAsync(user);
            return new UserDTO(user.Email, user.DisplayName, Token);
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
            {
                var Token = await CreateTokenAsync(User);

                return Result<UserDTO>.Ok(new UserDTO(User.Email, User.DisplayName, Token));
            }

            var errors = IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();
            return Result<UserDTO>.Fail(errors);
        }

        // Method to Create JWT Token
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            // Clamis => USerEmail , USerName
            var Claims = new List<Claim>() { 
            
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!)
            };

            // Roles ?
            var Roles =await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim("roles", role));
            }
            // Secret Key 
            var SecretKey = _configuration["JWTOptions:SecretKey"];
            

            var Key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            // Signing Credentials
            var Cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            // Crate Token
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions: Issuer"] ,
                audience: _configuration["JWTOptions:Audience"],
                expires:DateTime.UtcNow.AddHours(1),
                claims:Claims,
                signingCredentials: Cred
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);



        }
    }
}
