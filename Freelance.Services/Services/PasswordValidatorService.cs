using Freelance.Services.Interfaces;
using Freelance.Shared.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freelance.Services.Services
{
    public class PasswordValidatorService : IPasswordValidator
    {
        public PasswordValidatorService(IOptions<IdentityOptions> optionsAccessor)
        {
            Options = optionsAccessor?.Value ?? new IdentityOptions();
        }

        public IdentityOptions Options { get; set; }

        public virtual List<PasswordValidatorStatus> Validate(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var errors = new List<PasswordValidatorStatus>();
            var options = Options.Password;
            if (string.IsNullOrWhiteSpace(password) || password.Length < options.RequiredLength)
            {
                errors.Add(PasswordValidatorStatus.PasswordTooShort);
            }

            if (options.RequireNonAlphanumeric && password.All(IsLetterOrDigit))
            {
                errors.Add(PasswordValidatorStatus.PasswordRequiresNonAlphanumeric);
            }
            if (options.RequireDigit && !password.Any(IsDigit))
            {
                errors.Add(PasswordValidatorStatus.PasswordRequiresDigit);
            }
            if (options.RequireLowercase && !password.Any(IsLower))
            {
                errors.Add(PasswordValidatorStatus.PasswordRequiresLower);
            }
            if (options.RequireUppercase && !password.Any(IsUpper))
            {
                errors.Add(PasswordValidatorStatus.PasswordRequiresUpper);
            }
            if (options.RequiredUniqueChars >= 1 && password.Distinct().Count() < options.RequiredUniqueChars)
            {
                errors.Add(PasswordValidatorStatus.PasswordRequiresUniqueChars);
            }
            return errors;
        }

        public virtual bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

       
        public virtual bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        public virtual bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        public virtual bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }
    }
}
