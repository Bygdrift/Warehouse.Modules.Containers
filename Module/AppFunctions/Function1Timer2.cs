using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Module.AppFunctions
{
    public class Function1Timer2
    {
        [FunctionName("Function1Timer2")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");


        }
    }
}
