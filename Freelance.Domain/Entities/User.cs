using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class User : IdentityUser
    {
        public UserProfile UserProfile { get; set; }
    }
}
