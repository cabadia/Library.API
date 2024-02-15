using Library.Domain.Entities;
using Library.Domain.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories.DataMapping
{
    internal class BookMapping
    {
        internal static List<Book> ToList(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                throw new NoContentException();
            List<Book> books = new List<Book>();
            foreach(DataRow dataRow in dataTable.Rows)
            {
                Book book = new Book()
                {
                    Title = dataRow["Title"].ToString() ?? string.Empty,
                    Description = dataRow["Description"].ToString() ?? string.Empty,
                    Author = dataRow["Author"].ToString() ?? string.Empty
                };
                books.Add(book);
            }
            return books;
        }
    }
}
