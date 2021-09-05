using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TimesAzureFunction.Functions;
using todomigue.Common.Model;
using todomigue.Test.Helpers;
using Xunit;

namespace todomigue.Test.Tests
{
    public class WorkApiTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateWork_Should_Return_200()
        {
            //Arrenge
            MockCloudTable mockTodos = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            TodoWork todorequest = TestFactory.GetTodoRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(todorequest);

            //Act
            IActionResult response = await ConsolidateApi.CreateEntry(request, mockTodos, logger);
            //Assert
         }

        [Fact]
        public async void UpdateTodo_Should_Return_200()
        {
            // Arrenge
            MockCloudTable mockTodos = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            TodoWork todoRequest = TestFactory.GetTodoRequest();
            string todoId = Guid.NewGuid().ToString();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(todoRequest);

            // Act
            IActionResult response = await ConsolidateApi.UpdateEntrey(request, mockTodos, todoId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void GetAllTickets_should_Return_200()
        {
            //Arrenge
            MockCloudTable mockTodos = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            TodoWork todorequest = TestFactory.GetTodoRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(todorequest);

            //Act
            

            //Assert
        
        }


        [Fact]
        public async void GetInformationById_Should_Return_200()
        {
            // Arrenge
            MockCloudTable mockTodos = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            TodoWork todoRequest = TestFactory.GetTodoRequest();
            string todoId = Guid.NewGuid().ToString();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(todoRequest);

            // Act
            IActionResult response = await ConsolidateApi.UpdateEntrey(request,mockTodos, todoId.ToString(), logger);
            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void DeleteInformation_Should_Return_200()
        {
            // Arrenge
            MockCloudTable mockTodos = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            TodoWork todoRequest = TestFactory.GetTodoRequest();
            string todoId = Guid.NewGuid().ToString();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(todoRequest);

            // Act
            IActionResult response = await ConsolidateApi.UpdateEntrey(request, mockTodos, todoId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

    }
}
