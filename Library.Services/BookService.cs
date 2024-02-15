using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly string _connectionString;
        public BookService(IBookRepository bookRepository, string connectionString)
        {
            _bookRepository = bookRepository;
            _connectionString = connectionString;
        }

        public async Task<List<Book>> GetBooksAsync(CancellationToken cancellationToken)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                SqlTransaction transaction = connection.BeginTransaction();

                List<Book> books = await _bookRepository.GetBooksAsync(connection, transaction, cancellationToken);

                transaction.Commit();
                return books;
            }
        }
    }
}
