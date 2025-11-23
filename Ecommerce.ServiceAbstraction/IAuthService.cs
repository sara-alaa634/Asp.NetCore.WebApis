using Ecommerce.Shared.CommanResult;
using Ecommerce.Shared.DTOS.IdentityDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction
{
    public interface IAuthService
    {
        // DTOS
         // LoginDTO
         // RegisterDto
         // UserDTO
        //Login
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);

        // Register

        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
    }
}
