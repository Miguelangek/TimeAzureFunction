using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using TimesAzureFunction.Entities;

namespace TimesAzureFunction.Functions
{
    public static class ScheduledFunction
    {
        [FunctionName("ScheduledFunction")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
            [Table("TableWork", Connection = "AzureWebJobsStorage")] CloudTable todoTable,
            ILogger log)
        {
            log.LogInformation($"Deleting completed function executed at: {DateTime.Now}");

            string filter = TableQuery.GenerateFilterConditionForBool("IsCompleted", QueryComparisons.Equal, true);
            TableQuery <WorkEntity>  query = new  TableQuery<WorkEntity>().Where(filter);

        }
    }
}
