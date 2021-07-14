using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Request
{
    public class UserCategoryModel
    {
        public string UserId { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
