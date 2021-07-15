using Freelance.Shared.Enumerations;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Request
{
    public class RoleModel : PaginationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public EntityStatus Status { get; set; }

        public RoleModel(string name)
        {
            Name = name;
        }
    }
}
