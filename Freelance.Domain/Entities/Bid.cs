using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class Bid : CommonFields
    {
        public string Comment { get; set; }
        public double Rate { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int JobId { get; set; }
        public JobOffer Job { get; set; }
    }
}
