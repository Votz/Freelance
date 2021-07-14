using Freelance.Shared.Enumerations;

namespace Freelance.Services.Models.Response
{
    public class NotificationViewModel
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public UserType UserType { get; set; }
        public NotificationType NotificationType { get; set; }
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
    }
}
