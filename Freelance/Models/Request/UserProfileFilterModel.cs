using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class UserProfileFilterModel : PaginationModel
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string UserBio { get; set; }
        public double Rating { get; set; }
        public double HourRate { get; set; }
        public string Education { get; set; }
        public string Languages { get; set; }
        public string UserId { get; set; }
    }
}
