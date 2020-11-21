using System;

namespace ThisNetWorks.OrchardCore.Azure.Queues
{
    public interface IAzureQueueResolver
    {
        IAzureQueueClient ResolveQueue(string queueName, Type type);
    }
}