using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class Category : CommonFields
    {
        public string Name { get; set; }
        public ICollection<UserCategory> UserCategories { get; set; }
        public ICollection<JobCategory> JobCategories { get; set; }
    }
}
