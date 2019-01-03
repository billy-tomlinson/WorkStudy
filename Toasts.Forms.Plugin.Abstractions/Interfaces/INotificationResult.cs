using WorkStudy.Abstractions.Options;

namespace WorkStudy.Abstractions.Interfaces
{
    public interface INotificationResult
    {
        NotificationAction Action { get; set; }
        int Id { get; set; }
    }
}
