using Freelance.Services.Models.Request;
using Freelance.Shared.Enumerations;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Models.Request
{
    public class JobOfferFilterModel : PaginationModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public JobStatus JobStatus { get; set; }
        public int EmployerId { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}
