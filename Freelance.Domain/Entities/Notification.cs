using Freelance.Shared.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Domain.Entities
{
    public class Notification : CommonFields
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public UserType UserType { get; set; }
        public NotificationType NotificationType { get; set; }
        public string UserId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int JobId { get; set; }
        public JobOffer Job { get; set; }
    }
}
