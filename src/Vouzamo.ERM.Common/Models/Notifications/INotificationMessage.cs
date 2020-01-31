namespace Vouzamo.ERM.Common.Models.Notifications
{
    public interface INotificationMessage
    {
        string Title { get; }
        string Message { get; }
    }
}
