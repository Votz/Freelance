using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class ChangeJobStatusRequestModel
    {
        public JobStatus Status { get; set; }
        public int JobId { get; set; }
    }
}
