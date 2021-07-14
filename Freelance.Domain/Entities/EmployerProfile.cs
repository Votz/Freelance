using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class EmployerProfile : CommonFields
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
