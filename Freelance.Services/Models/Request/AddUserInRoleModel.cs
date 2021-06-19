using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Request
{
    public class AddUserInRoleModel
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
    }
}