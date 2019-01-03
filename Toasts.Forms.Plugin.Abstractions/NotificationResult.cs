using WorkStudy.Abstractions.Interfaces;

namespace WorkStudy.Abstractions.Options
{
    public class NotificationResult : INotificationResult
    {
        public NotificationAction Action { get; set; }
        public int Id { get; set; }
    }
}
