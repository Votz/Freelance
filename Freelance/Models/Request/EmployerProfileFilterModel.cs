using Freelance.Shared.Models;

namespace Freelance.Api.Models.Request
{
    public class EmployerProfileFilterModel : PaginationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public string UserId { get; set; }
    }
}
