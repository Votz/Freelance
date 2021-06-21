using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Response
{
    public class GetUserTokenValidityModel
    {
        public DateTime ValidTo { get; set; }
        public bool IsValid { get; set; }

        public GetUserTokenValidityModel(bool isValid,DateTime expires)
        {
            ValidTo = expires;
            IsValid = isValid;
        }
        public GetUserTokenValidityModel()
        {
            IsValid = false;
        }
    }
}
