using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Repositories.DataMapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        public async Task<List<Book>> GetBooksAsync(SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken)
        {
            using(SqlCommand command = new SqlCommand("[dbo].[pGetBooks]", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                DataTable dataTable = new DataTable();
                dataTable.Load(await command.ExecuteReaderAsync(cancellationToken));
                return BookMapping.ToList(dataTable);
            }
        }
    }
}
