using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TimesAzureFunction.Functions;
using todomigue.Test.Helpers;
using Xunit;

namespace todomigue.Test.Tests
{
   public class ScheduledFunctionTest
    {
        
        [Fact]
        public void ScheduledFunction_Should_Log_Message()
        {
            //Arrange
            MockCloudTable mockTodos = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            ListLogger logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
            // Act
            ScheduledFunction.Run(null, mockTodos, logger);
            string message = logger.Logs[0];
            //Assert
            Assert.Contains("Deleting completed", message);
        }
    }
}
