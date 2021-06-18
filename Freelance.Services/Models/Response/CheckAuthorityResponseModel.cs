using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Response
{
    public class CheckAuthorityResponseModel
    {
        public bool IsAuthorized { get; set; }
        public string Username { get; set; }
    }
}
