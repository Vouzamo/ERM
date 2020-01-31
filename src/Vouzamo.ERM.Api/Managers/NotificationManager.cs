using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Models.Notifications;

namespace Vouzamo.ERM.Api.Managers
{
    public class NotificationManager : INotificationManager
    {
        protected ISubject<INotificationMessage> MessageStream { get; }

        public NotificationManager()
        {
            MessageStream = new Subject<INotificationMessage>();
        }

        public Task<IObservable<INotificationMessage>> MessagesAsync()
        {
            var observable = MessageStream.AsObservable();

            return Task.FromResult(observable);
        }

        public Task Notify(INotificationMessage message)
        {
            MessageStream.OnNext(message);

            return Task.CompletedTask;
        }
    }
}
