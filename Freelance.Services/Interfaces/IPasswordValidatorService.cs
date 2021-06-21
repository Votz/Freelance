using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Interfaces
{
    public interface IPasswordValidatorService
    {
        
        List<string> Validate(string password);
    }
}
