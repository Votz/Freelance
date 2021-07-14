using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Request
{
    public class EmployerProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public string UserId { get; set; }
    }
}
