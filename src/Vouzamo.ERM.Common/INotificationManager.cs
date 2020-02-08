using System;
using System.Threading.Tasks;
using Vouzamo.ERM.Common.Models.Notifications;

namespace Vouzamo.ERM.Common
{
    public interface INotificationManager
    {
        Task Notify(INotificationMessage message);
        Task<IObservable<INotificationMessage>> MessagesAsync(string recipient);
    }
}
