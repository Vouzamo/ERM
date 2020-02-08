using System;

namespace Vouzamo.ERM.Common.Models.Notifications
{
    public class SimpleNotificationMessage : INotificationMessage
    {
        public Guid Id { get; }
        public string Recipient { get; }
        public string Title { get; }
        public string Message { get; }
        public Severity Severity { get; }

        protected SimpleNotificationMessage()
        {
            Id = Guid.NewGuid();
        }

        public SimpleNotificationMessage(string title, string message, Severity severity, string recipient = null) : this()
        {
            Title = title;
            Message = message;
            Severity = severity;
            Recipient = recipient;
        }
    }
}
