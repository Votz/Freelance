using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Shared.Models
{
    public class ApiResponse
    {
        public int Status { get; set; }
        public string StatusMessage { get; set; }
        public ApiError Errors { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Model { get; set; }
    }

    public class ApiError
    {
        public List<string> ErrorMessages { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
