using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccess;

namespace Bookish
{
    class Program
    {
        static void Main(string[] args)
        {
            var books2 = GetBooks();
            foreach (var book in books2)
            {
                Console.WriteLine(book.Describe());
            }

            while (true)
            {

            }
            
        }

        public static List<Book> GetBooks()
        {
            var aa = new Class1();
            IDbConnection db = new SqlConnection("Data Source=PEKINGESE\\MSSQLSERVER01;Initial Catalog=master;Integrated Security=True");
            var books = db.Query("SELECT * FROM [Books]");
            var books2 = new List<Book>();
            foreach (var book in books)
            {
                books2.Add(new Book(book.Id, book.author, book.title, book.ISBN));
            }

            var borrows = db.Query("SELECT * FROM [Borrows]");
            var borrows2 = new List<Borrow>();
            foreach (var borrow in borrows)
            {
                borrows2.Add(new Borrow(borrow.Id, borrow.user_id, borrow.book_id, borrow.date_due));
            }

            foreach (var borrow in borrows2)
            {
                foreach (var book in books2)
                {
                    if (borrow.id == book.id)
                    {
                        book.extraInfo = "Book borrowed, due in " + borrow.date_due ;
                    }
                }
            }

            return books2;
        }

    
    }
}

/**
 * Books:
 * id: int
 * title: string
 * author: string
 * ISBN: string
 *
 * Users:
 * id: int
 * name: string
 * password_hash: string
 *
 * Borrows:
 * id: int
 * user_id: int
 * book_id: int
 * date_due: string
 *
 *
 */
