using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class UserCategory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
