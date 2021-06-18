using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Interfaces
{
    public interface IPasswordValidator
    {
        
        List<PasswordValidatorStatus> Validate(string password);
    }
}
