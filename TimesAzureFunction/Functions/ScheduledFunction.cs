using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using TimesAzureFunction.Entities;
using todomigue.Common.Response;

namespace TimesAzureFunction.Functions
{
    public static class ScheduledFunction
    {
        [FunctionName("ScheduledFunction")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
             [Table("TableWork", Connection = "AzureWebJobsStorage")] CloudTable Tablework ,
            [Table("ConsolidateEmployees", Connection = "AzureWebJobsStorage")] CloudTable validateTimes,
            ILogger log)
        {
            TableQuery<WorkEntity> query = new TableQuery<WorkEntity>();
            TableQuerySegment<WorkEntity> validation = await Tablework.ExecuteQuerySegmentedAsync(query, null);
          
            List<WorkEntity> workEntity = validation.Where(x => x.WorkingTime.ToString("dd/M/yyyy") == DateTime.UtcNow.ToString("dd/M/yyyy") && x.Consolidate == false)
                                                                                                                                               .OrderBy(x => x.EmployeeId)
                                                                                                                                               .ThenBy(x => x.WorkingTime)
                                                                                                                                                .ToList();


            if (workEntity.Count == 0) return;
            for(int j = 0; j<workEntity.Count; j++)
            {
                if (workEntity[j] != null && workEntity[j + 1] != null &&
                       workEntity[j].EmployeeId.Equals(workEntity[j + 1].EmployeeId)
                   && workEntity[j].WorkingTime.ToString("dd/M/yyyy").Equals(workEntity[j + 1].WorkingTime.ToString("dd/M/yyyy")))
                {

                    TimeSpan difference = workEntity[j + 1].WorkingTime - workEntity[j].WorkingTime;
                    double differenceminutes = (Math.Truncate(difference.TotalMinutes * 10000) / 10000);

                    ConsolidatedEmployees consolidatedemployee = new ConsolidatedEmployees
                    {
                        Date = DateTime.UtcNow,
                        ETag = "*",
                        PartitionKey = "CONSOLIDATEDWORKTIME",
                        RowKey = Guid.NewGuid().ToString(),
                        EmployeeId = workEntity[j].EmployeeId,
                        MinutesWorked = differenceminutes,


                    };
                    TableOperation addinformation = TableOperation.Insert(consolidatedemployee);
                    await validateTimes.ExecuteAsync(addinformation);

                }
            }


        }
    }
}
