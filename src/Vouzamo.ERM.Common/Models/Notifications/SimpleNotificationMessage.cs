namespace Vouzamo.ERM.Common.Models.Notifications
{
    public class SimpleNotificationMessage : INotificationMessage
    {
        public string Title { get; }
        public string Message { get; }

        protected SimpleNotificationMessage()
        {

        }

        public SimpleNotificationMessage(string title, string message) : this()
        {
            Title = title;
            Message = message;
        }
    }
}
