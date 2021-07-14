using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class UserCategoryFilterRequestModel : PaginationModel
    {
        public string UserId { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
