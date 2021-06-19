using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class AddUserInRoleRequest
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
    }
}
