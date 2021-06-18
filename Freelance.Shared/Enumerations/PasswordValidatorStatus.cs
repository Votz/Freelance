using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Shared.Enumerations
{
    public enum PasswordValidatorStatus
    {
        PasswordTooShort,
        PasswordRequiresNonAlphanumeric,
        PasswordRequiresDigit,
        PasswordRequiresLower,
        PasswordRequiresUpper,
        PasswordRequiresUniqueChars
    }
}
