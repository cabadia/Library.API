using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksAsync(SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken);
    }
}
