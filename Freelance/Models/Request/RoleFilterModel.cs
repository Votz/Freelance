using Freelance.Shared.Enumerations;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class RoleFilterModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public EntityStatus Status { get; set; }
    }
}
