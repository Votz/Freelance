using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Request
{
    public class UserCategoryFilterModel : PaginationModel
    {
        public string UserId { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
