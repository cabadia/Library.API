using Library.Domain.Entities.Extensions;
using Library.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Library.API.Controllers
{
    [RoutePrefix("Book")]
    public class BookController : ApiController
    {
        private readonly IBookService _bookService;
        private readonly ILogService _logService;
        public BookController(IBookService bookService, ILogService logService)
        {
            _bookService = bookService;
            _logService = logService;
        }
        
        [HttpGet]
        [Route("List")]
        public async Task<IHttpActionResult> GetBooksAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(await _bookService.GetBooksAsync(cancellationToken));
            }
            catch (NoContentException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch(Exception ex)
            {
                _logService.LogException(ex);
                return InternalServerError(ex);
            }
        }
    }
}