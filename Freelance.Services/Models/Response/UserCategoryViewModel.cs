using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Response
{
    public class UserCategoryViewModel
    {
        public List<CategoryIdNameModel> Categories { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }

    public class CategoryIdNameModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
