using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Request
{
    public class ChangeJobOfferStatus
    {
        public JobStatus Status { get; set; }
        public int JobId { get; set; }
    }
}
