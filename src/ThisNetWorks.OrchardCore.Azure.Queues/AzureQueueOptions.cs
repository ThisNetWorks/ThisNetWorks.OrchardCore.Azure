using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace ThisNetWorks.OrchardCore.Azure.Queues
{
    public class AzureQueueOptions
    {
        private List<AzureQueueOption> _azureQueueOptions = new List<AzureQueueOption>();

        private Dictionary<string, AzureQueueOption> _lookup;
        public IReadOnlyDictionary<string, AzureQueueOption> Lookup => _lookup ??= _azureQueueOptions.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

        public string ConnectionString { get; set; }
        public bool CreateQueues { get; set; }

        internal void AddAzureQueue(AzureQueueOption azureQueueOption)
        {
            _azureQueueOptions.Add(azureQueueOption);
        }

    }

    public class AzureQueueOption
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Type Type { get; set; }
    }
}

