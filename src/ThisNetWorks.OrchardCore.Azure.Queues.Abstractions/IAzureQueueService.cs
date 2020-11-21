using System;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues.Models;

namespace ThisNetWorks.OrchardCore.Azure.Queues
{
    public interface IAzureQueueService
    {
        Task<Response<SendReceipt>> SendMessageAsync(string queueName, Type queueType, string messageText);
    }
}

