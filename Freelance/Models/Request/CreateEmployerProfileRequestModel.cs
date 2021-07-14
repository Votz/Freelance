using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class CreateEmployerProfileRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public string UserId { get; set; }
    }
}
