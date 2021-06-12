using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Response
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
