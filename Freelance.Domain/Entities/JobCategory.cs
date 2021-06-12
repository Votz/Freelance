using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class JobCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }
    }
}
