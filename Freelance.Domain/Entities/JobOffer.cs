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
        public double WorkDuration { get; set; }
        public int Salary { get; set; }
        public int HourRate { get; set; }
        public JobStatus JobStatus { get; set; }
        public int EmployerId { get; set; }
        public virtual EmployerProfile Employer { get; set; }
        public virtual ICollection<JobCategory> JobCategories { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
