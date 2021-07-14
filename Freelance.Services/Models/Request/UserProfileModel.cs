using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Request
{
    public class UserProfileModel
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
