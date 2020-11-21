using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ThisNetWorks.OrchardCore.Azure.Queues
{
    public class AzureQueueService : IAzureQueueService
    {
        private readonly AzureQueueOptions _azureQueueOptions;
        private readonly IAzureQueueResolver _azureQueueResolver;

        public AzureQueueService(
            IOptions<AzureQueueOptions> azureQueueOptions, 
            IAzureQueueResolver azureQueueResolver)
        {
            _azureQueueOptions = azureQueueOptions.Value;
            _azureQueueResolver = azureQueueResolver;
        }

        public Task<Response<SendReceipt>> SendMessageAsync(string queueName, Type queueType, string messageText)
        {
            var queue = _azureQueueResolver.ResolveQueue(queueName, queueType);

            return queue.SendMessageAsync(messageText);
        }
    }

    public static class AzureQueueServiceExtensions
    {        
        public static Task<Response<SendReceipt>> SendMessageAsync<T>(this IAzureQueueService azureQueueService, T message)
            => azureQueueService.SendMessageAsync(typeof(T), message);

        public static async Task<Response<SendReceipt>> SendMessageAsync(this IAzureQueueService azureQueueService, string queueName, JObject message)
        {
            // TODO pool.
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    await message.WriteToAsync(jsonWriter);
                }
            }

            var messageText = Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString()));

            return await azureQueueService.SendMessageAsync(queueName, typeof(JObject), messageText);

        }

        public static Task<Response<SendReceipt>> SendMessageAsync(this IAzureQueueService azureQueueService, Type type, object message)
        {
            var messageText = JsonConvert.SerializeObject(message);

            messageText = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageText));

            return azureQueueService.SendMessageAsync(type.Name, type, messageText);
        }            
    }  
}

