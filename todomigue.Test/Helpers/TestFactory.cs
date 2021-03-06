using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TimesAzureFunction.Entities;
using todomigue.Common.Model;

namespace todomigue.Test.Helpers
{
   public class TestFactory
    {

        public static WorkEntity GetTodoEntity()
        {
            return new WorkEntity
            {
                ETag = "*",
                PartitionKey = "TODO",
                RowKey = Guid.NewGuid().ToString(),
                WorkingTime = DateTime.UtcNow,
                Consolidate = false,
                Type ="0"
                
               
               

            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid todoId, TodoWork todorequest)
        {
            string request = JsonConvert.SerializeObject(todorequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"{todoId}"
            };

        }

        public static DefaultHttpRequest CreateHttpRequest(Guid todoId)
        {
           
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
               
                Path = $"{todoId}"
            };

        }


        public static DefaultHttpRequest CreateHttpRequest(TodoWork todorequest)
        {
            string request = JsonConvert.SerializeObject(todorequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
            
            };

        }


        public static DefaultHttpRequest CreateHttpRequest(string todoId)
        {

            return new DefaultHttpRequest(new DefaultHttpContext());
        }


        public static TodoWork GetTodoRequest()
        {
            return new TodoWork
            {
                WorkingTime = DateTime.UtcNow,
                Consolidate = false,
                Type = "0"
            };
            
        }


        public  static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if(type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }
            return logger;
        }
    }
}
