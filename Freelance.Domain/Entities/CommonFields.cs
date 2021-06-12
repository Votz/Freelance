using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class CommonFields
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifyDate { get; set; }
        public EntityStatus Status { get; set; }
        public string ModifierId { get; set; }
    }
}
