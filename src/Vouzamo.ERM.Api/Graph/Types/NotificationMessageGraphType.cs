using GraphQL.Types;
using Vouzamo.ERM.Common.Models.Notifications;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public class NotificationMessageGraphType : ObjectGraphType<INotificationMessage>
    {
        public NotificationMessageGraphType()
        {
            Field(o => o.Id, type: typeof(IdGraphType));
            Field(o => o.Title);
            Field(o => o.Message);
            Field(o => o.Severity, type: typeof(SeverityEnumerationGraphType));
            Field(o => o.Recipient);
        }
    }
}
