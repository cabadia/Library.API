using Library.API;
using Library.API.Controllers;
using Library.Domain.Entities;
using Library.Domain.Entities.Extensions;
using Library.Service.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Library.API.Tests.Controllers
{
    [TestClass]
    public class BookControllerTest
    {
        private BookController _bookController;
        private Mock<IBookService> _bookServiceMock;
        private Mock<ILogService> _logServiceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _bookServiceMock = new Mock<IBookService>();
            _logServiceMock = new Mock<ILogService>();
            _bookController = new BookController(_bookServiceMock.Object, _logServiceMock.Object);
        }

        [TestMethod]
        public async Task GetBooksAsync_ReturnsSuccesfulResults()
        {
            //Arrange
            var books = new List<Book> { new Book { Title = "Book 1" }, new Book { Title = "Book 2" } };
            _bookServiceMock.Setup(x => x.GetBooksAsync(It.IsAny<CancellationToken>())).ReturnsAsync(books);

            //Act
            var result = await _bookController.GetBooksAsync();

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<Book>>));
            Assert.IsNotNull((result as OkNegotiatedContentResult<List<Book>>).Content);
        }

        [TestMethod]
        public async Task GetBooksAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            //Arrange
            _bookController.ModelState.AddModelError("error", "some error");

            //Act
            var result = await _bookController.GetBooksAsync();

            //Assert
            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task GetBooksAsync_ReturnsNoContent_WhenNoContentExceptionIsThrown()
        {
            //Arrange
            _bookServiceMock.Setup(x => x.GetBooksAsync(It.IsAny<CancellationToken>())).Throws(new NoContentException());

            //Act
            var result = await _bookController.GetBooksAsync();

            //Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public async Task GetBooksAsync_ReturnsInternalServerError_WHenExceptionIsThrown()
        {
            //Arrange
            _bookServiceMock.Setup(x => x.GetBooksAsync(It.IsAny<CancellationToken>())).Throws(new Exception());

            //Act
            var result = await _bookController.GetBooksAsync();

            //Assert
            _logServiceMock.Verify(l => l.LogException(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(ExceptionResult));            
        }
    }
}
