using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "იმეილი აუცილებელია")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="იმეილი აუცილებელია")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserType UserType { get; set; }
    }
}
