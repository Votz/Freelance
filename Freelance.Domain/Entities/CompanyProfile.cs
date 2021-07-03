using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class CompanyProfile : CommonFields
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double WorkDuration { get; set; }
        public int Salary { get; set; }
        public int HourRate { get; set; }
        public Currency Currency { get; set; }


    }
}
