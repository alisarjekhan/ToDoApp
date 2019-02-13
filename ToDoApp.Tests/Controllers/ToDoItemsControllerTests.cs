using DataLayer;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoApp.Controllers;
using FluentAssertions;
using System.Web.Http.Results;
using ToDoLib;
using System.Net.Http;
using System.Security.Principal;

namespace ToDoApp.Tests.Controllers
{
    [TestFixture]
    public class ToDoItemsControllerTests//:ApiController
    {
        
        public ToDoItemsControllerTests()
        {
            AutoMapperMappings.Initialize();
        }

        [Test]
        public void When_GetAllToDoItems_WithNoDataInDB_Then_ReturnNotFoundResult()
        {
            // Arrange
            var repoService = Substitute.For<IToDoRepoService>();
            repoService.GetToDoItems().Returns(new List<ToDoItem>());
            var toDoItemsController = new ToDoItemsController(repoService);

            // Act
            var result = toDoItemsController.GetAllToDoItems();

            // Assert
            repoService.Received(1).GetToDoItems();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void When_GetAllToDoItems_WithData_Then_ReturnOkResultWithData()
        {
            // Arrange
            var repoService = Substitute.For<IToDoRepoService>();
            var todoItemFromDb = new ToDoItem { ToDoItemId = 1 };
            repoService.GetToDoItems().Returns(new List<ToDoItem>() { todoItemFromDb });
            var toDoItemsController = new ToDoItemsController(repoService);

            // Act
            var result = toDoItemsController.GetAllToDoItems();

            // Assert
            repoService.Received(1).GetToDoItems();
            result.Should().BeOfType<OkNegotiatedContentResult<IList<ToDoItemDto>>>();
            (result as OkNegotiatedContentResult<IList<ToDoItemDto>>).Content.First().ToDoItemId.Should().Be(todoItemFromDb.ToDoItemId);
        }

        [Test]
        public void When_GetAllToDoItems_WithException_Then_ReturnExceptionResult()
        {
            // Arrange
            var repoService = Substitute.For<IToDoRepoService>();
            var exception = new Exception("TestException");
            repoService.GetToDoItems().Returns(x => throw exception);
            var toDoItemsController = new ToDoItemsController(repoService);

            // Act
            var result = toDoItemsController.GetAllToDoItems();

            // Assert
            repoService.Received(1).GetToDoItems();
            result.Should().BeOfType<ExceptionResult>();
            (result as ExceptionResult).Exception.Should().Be(exception);
        }

        [Test]
        public void When_PostNewToDoItem_Then_AddTodoItem_And_ReturnCreatedNegotiatedContentResult()
        {
            // Arrange
            var repoService = Substitute.For<IToDoRepoService>();

            var cachedItem = new ToDoItem();
            //Below line simulates the add process where db model's id is updated with the identity id
            repoService.WhenForAnyArgs(t => t.AddToDoItem(cachedItem)).Do(t => { var arg = t.Arg<ToDoItem>(); arg.ToDoItemId = 1; cachedItem = arg; });
           
            var toDoItemsController = new ToDoItemsController(repoService)
            {
                User = Substitute.For<IPrincipal>()
            };

            toDoItemsController.User.IsInRole("admin").Returns(true);
            toDoItemsController.Request = Substitute.For<HttpRequestMessage>();
            toDoItemsController.Request.RequestUri = new Uri("BaseUri",UriKind.RelativeOrAbsolute);

            var dto = new ToDoItemDto { ToDoItemId = 0 };

            // Act
            var result = toDoItemsController.PostNewToDoItem(dto);

            // Assert
            repoService.ReceivedWithAnyArgs(1).AddToDoItem(Substitute.For<ToDoItem>());
            result.Should().BeOfType<CreatedNegotiatedContentResult<ToDoItemDto>>();

            var cnresult = result as CreatedNegotiatedContentResult<ToDoItemDto>;
            cnresult.Content.ToDoItemId.Should().Be(cachedItem.ToDoItemId);
            cnresult.Location.OriginalString.Should().Be($"{toDoItemsController.Request.RequestUri}/{1}");
        }

        [TestCase(false, typeof(UnauthorizedResult))]
        [TestCase(true, typeof(CreatedNegotiatedContentResult<ToDoItemDto>))]
        public void When_PostNewToDoItem_And_UserIsNotInAdminRole_Then_ReturnUnauthorizedResult_ElseCreatedResult(bool isAdminRole, Type expectedResultType)
        {
            // Arrange
            var repoService = Substitute.For<IToDoRepoService>();

            var toDoItemsController = new ToDoItemsController(repoService)
            {
                User = Substitute.For<IPrincipal>()
            };

            toDoItemsController.User.IsInRole("admin").Returns(isAdminRole);
            toDoItemsController.Request = Substitute.For<HttpRequestMessage>();
            toDoItemsController.Request.RequestUri = new Uri("BaseUri", UriKind.RelativeOrAbsolute);

            var dto = new ToDoItemDto { ToDoItemId = 0 };

            // Act
            var result = toDoItemsController.PostNewToDoItem(dto);

            // Assert
            result.Should().BeOfType(expectedResultType);
        }

    }
}
