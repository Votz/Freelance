using Freelance.Services.Models.Request;
using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Models.Response
{
    public class JobOfferViewModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double WorkDuration { get; set; }
        public int Salary { get; set; }
        public int HourRate { get; set; }
        public JobStatus JobStatus { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}
