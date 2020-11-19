using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues.Models;

namespace ThisNetWorks.OrchardCore.Azure.Queues
{
    /// <summary>
    /// A registration of an azure queue client.
    /// </summary>
    public interface IAzureQueueClient
    {
        /// <summary>
        /// The name of the queue.
        /// Must match Azure Queue naming conventions
        /// </summary>
        string Name { get; }
        Task<Response<SendReceipt>> SendMessageAsync(string messageText);
        Task<Response> CreateIfNotExistsAsync(IDictionary<string, string> metadata = default, CancellationToken cancellationToken = default);
    }
}

