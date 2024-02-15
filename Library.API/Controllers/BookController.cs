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
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
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
                //todo: log it.
                return InternalServerError(ex);
            }

        }
    }
}