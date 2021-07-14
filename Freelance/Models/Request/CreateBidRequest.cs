using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class CreateBidRequest
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Comment { get; set; }
        public double Rate { get; set; }
        public int UserProfileId { get; set; }
        public int JobId { get; set; }
        public EntityStatus Status { get; set; }
    }
}
