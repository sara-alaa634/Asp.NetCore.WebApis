using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.DTOS.IdentityDTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    public class AuthenticationController:ApiBaseController
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }// Register in Program.cs

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login( LoginDTO loginDTO)
        {
            var result = await _authService.LoginAsync(loginDTO);
            return HandleResult(result);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await _authService.RegisterAsync(registerDTO);
            return HandleResult(result);
        }


        // Get: BAseuse/ api/Authentication/emailExsist

        [Authorize]
        [HttpGet("emailExsist")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var Result = await _authService.CheckEmailAsync(email);
            return Ok(Result);
        }
        // Get: BAseuse/ api/Authentication/CurrentUser

        [Authorize]
        [HttpGet("CurrentUser")]

        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Result = await _authService.GetUserByEmailAsync(Email!);
            return HandleResult(Result);
        }


    }

}
