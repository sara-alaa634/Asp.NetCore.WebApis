using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.CommanResult
{
    public class Error
    {
       

        public string Code { get; }
        public string Description { get; }

        public ErrorType Type { get;  }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        // Methods
        public static Error Failure(string code="General Failure", string description= "General Failure Occured !")
        {
            return new Error(code, description, ErrorType.Failure);
        }

        public static Error Validation(string code = "Validation Failure", string description = "General Validation Occured !")
        {
            return new Error(code, description, ErrorType.Validation);
        }

        public static Error NotFound(string code = "Not Found", string description = "Error 404 Not Found !")
        {
            return new Error(code, description, ErrorType.NotFound);
        }

        public static Error Unauthorized(string code = "Unauthorized Error", string description = "Error Unauthorized Occured !")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }

        public static Error Forbidden(string code = "Forbidden Error", string description = "Error Forbidden Occured !")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }
        public static Error invalidCredentials(string code = "Invalid Credentials", string description = "Error Invalid Credentials Occured !")
        {
            return new Error(code, description, ErrorType.invalidCredentials);
        }

    

    }
}
