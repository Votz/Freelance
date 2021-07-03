using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class UserProfile : CommonFields
    {
        public string Location { get; set; }
        public string UserBio { get; set; }
        public double Rating { get; set; }
        public double HourRate { get; set; }
        public string Education { get; set; }
        public string Languages { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<UserCategory> UserCategories { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

    }
}
