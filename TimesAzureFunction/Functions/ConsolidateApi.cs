using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using todomigue.Common.Model;
using todomigue.Common.Response;
using TimesAzureFunction.Entities;

namespace TimesAzureFunction.Functions
{
    public static class ConsolidateApi
    {
        [FunctionName(nameof(CreateEntry))]  
        public static async Task<IActionResult> CreateEntry(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "TableWork")] HttpRequest req,
            [Table("TableWork",Connection = "AzureWebJobsStorage")] CloudTable todoTable,
            ILogger log)
        {
            log.LogInformation("Recieved a new Request");

           

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TodoWork todowork = JsonConvert.DeserializeObject<TodoWork>(requestBody);

            if (string.IsNullOrEmpty(todowork?.EmployeeId))
            {
                return new BadRequestObjectResult(new ResponseWork
                {
                    IsSucces = false,
                    Message = "there must be a worker entry time EmployeeId"

                });
            }


            WorkEntity workEntity = new WorkEntity
            {
                WorkingTime = DateTime.UtcNow,
                ETag = "*",
                Consolidate = false,
                PartitionKey = "TODO",
                RowKey = Guid.NewGuid().ToString(),
                EmployeeId = todowork.EmployeeId
                
            };

            TableOperation addOperation = TableOperation.Insert(workEntity);

            await todoTable.ExecuteAsync(addOperation);

            string message = "new working hour in table";
            log.LogInformation(message);






            return new OkObjectResult(new ResponseWork
            {
                IsSucces = true,
                Message = message,
                Result = workEntity

            });
        }




        [FunctionName(nameof(UpdateEntrey))]
        public static async Task<IActionResult> UpdateEntrey(
           [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "TableWork/{id}")] HttpRequest req,
           [Table("TableWork", Connection = "AzureWebJobsStorage")] CloudTable todoTable,
           string id,
           ILogger log)
        {
            log.LogInformation($"Update for Employee: {id}, received");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            TodoWork todowork = JsonConvert.DeserializeObject<TodoWork>(requestBody);

            //validate employee id
            TableOperation findOperation = TableOperation.Retrieve<WorkEntity>("TODO", id);
            TableResult findResult = await todoTable.ExecuteAsync(findOperation);

            if(findResult.Result == null)
            {
                return new BadRequestObjectResult(new ResponseWork
                {
                    IsSucces = false,
                    Message = "EmployeeId not found"

                });

            }

            // update work
            WorkEntity workEntity = (WorkEntity)findResult.Result;
            workEntity.Consolidate = todowork.Consolidate;

            if (!string.IsNullOrEmpty(todowork.EmployeeId))
            {
                workEntity.EmployeeId = todowork.EmployeeId;
            }

          

            TableOperation addOperation = TableOperation.Replace(workEntity);

            await todoTable.ExecuteAsync(addOperation);

            string message = $"Employee : {id}, update in table";
            log.LogInformation(message);






            return new OkObjectResult(new ResponseWork
            {
                IsSucces = true,
                Message = message,
                Result = workEntity

            });
        }
    }

}
