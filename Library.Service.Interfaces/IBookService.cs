using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Service.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync(CancellationToken cancellationToken);
    }
}
