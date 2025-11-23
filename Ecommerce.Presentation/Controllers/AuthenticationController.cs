using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.DTOS.IdentityDTOS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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




    }

}
