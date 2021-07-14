using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Shared.Models
{
    public class PaginationResponseModel<T>
    {
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public List<T> Model { get; set; }

        public PaginationResponseModel(int totalCount, List<T> model)
        {
            TotalCount = totalCount;
            Model = model;
        }
    }
}
