using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Shared.Models
{
    public class PaginationModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPageCount { get; set; }
        public int TotalRecordCount { get; set; }
    }
}
