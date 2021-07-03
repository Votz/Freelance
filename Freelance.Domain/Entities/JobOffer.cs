using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class JobOffer : CommonFields
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public JobStatus JobStatus { get; set; }
        public virtual UserProfile
        public virtual ICollection<JobCategory> JobCategories { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
