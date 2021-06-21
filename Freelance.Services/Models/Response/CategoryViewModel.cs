using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Response
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifyDate { get; set; }
        public EntityStatus Status { get; set; }
    }
}
