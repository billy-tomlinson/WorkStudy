using System;
namespace WorkStudy.Services
{
    public interface ILocalNotificationService
    {
        void LocalNotification(string title, string body, int id, DateTime notifyTime);
        void DisableLocalNotification(string title, string body, int id, DateTime notifyTime);
        void Cancel(int id);
    }
}
