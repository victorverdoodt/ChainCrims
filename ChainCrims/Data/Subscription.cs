using ChainCrims.Models;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System.Threading;
using System.Threading.Tasks;

namespace ChainCrims.Data
{
    public class Subscription
    {
        [SubscribeAndResolve]
        public async ValueTask<ISourceStream<Conta>> OnContaCriada([Service] ITopicEventReceiver eventReceiver, CancellationToken cancellationToken)
        {
            return await eventReceiver.SubscribeAsync<string, Conta>("ContaCriada", cancellationToken);
        }

    }
}
