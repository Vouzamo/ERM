using System;

namespace Vouzamo.ERM.Common.Models.Notifications
{
    public enum Severity
    {
        Success,
        Info,
        Warning,
        Error
    }

    public interface INotificationMessage
    {
        Guid Id { get; }
        string Recipient { get; }
        string Title { get; }
        string Message { get; }
        Severity Severity { get; }
    }
}
