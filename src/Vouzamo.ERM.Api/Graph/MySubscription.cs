using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using Vouzamo.ERM.Api.Graph.Types;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Models.Notifications;

namespace Vouzamo.ERM.Api.Graph
{
    public class MySubscription : ObjectGraphType
    {
        public MySubscription(INotificationManager manager)
        {
            Name = "Subscription";

            AddField(new EventStreamFieldType
            {
                Name = "notifications",
                Arguments = new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "recipient" }
                ),
                Type = typeof(NotificationMessageGraphType),
                Resolver = new FuncFieldResolver<INotificationMessage>(context => context.Source as INotificationMessage),
                AsyncSubscriber = new AsyncEventStreamResolver<INotificationMessage>(context => manager.MessagesAsync(context.GetArgument<string>("recipient")))
            });
        }
    }
}
