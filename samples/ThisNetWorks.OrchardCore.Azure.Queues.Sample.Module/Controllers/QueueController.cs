using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module.Models;
using ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module.ViewModels;

namespace ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module.Controllers
{
    [Admin]
    public class QueueController : Controller
    {
        private readonly IAzureQueueService _azureQueueService;
        public QueueController(IAzureQueueService azureQueueService)
        {
            _azureQueueService = azureQueueService;

        }

        public ActionResult Index()
        {
            return View(new FooQueueIn());
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(FooQueueIn model)
        {
            var result = await _azureQueueService.SendMessageAsync(model);

            return View(new SendMessageResultViewModel { MessageId = result.Value.MessageId });
        }
    }
}
