using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using Dapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Web.Models;

namespace Web.Controllers
{
    public class BookController : Controller
    {

        [HttpGet]
        [Route("{id}")]
        public void Borrow(int id)
        {
            var userId = User.Identity.GetUserId();
            BorrowBook(id, userId);
        }

        [HttpGet]
        [Route("{id}")]
        public void UnBorrow(int id)
        {
            UnBorrowBook(id);
        }

        [HttpPost]
        public void Add(BookViewModel bookToAdd)
        {
            IDbConnection db = new SqlConnection(connectionString);
            var books = GetAllBooks();
            var freshId = books.Select(bor => bor.id)
                              .Aggregate(0, (current, next) => Math.Max(current, next));
            for (var i = 0; i < bookToAdd.amount; i++)
            {
                freshId++;
                var bookObject = new
                {
                    id = freshId,
                    title = bookToAdd.Title,
                    author = bookToAdd.Author,
                    ISBN = bookToAdd.ISBN
                };
                db.Execute("INSERT INTO [Books] (id, title, author, ISBN) VALUES " +
                           "(@id, @title, @author, @ISBN)", bookObject);
            }
            
            db.Close();
        }


        private static void BorrowBook(int bookId, string userId)
        {
            IDbConnection db = new SqlConnection(connectionString);
            var borrows = GetAllBorrows();
            var freshId = borrows.Select(bor => bor.id)
                              .Aggregate(0, (current, next) => Math.Max(current, next))
                          + 1;
            var borrow = new
            {
                id = freshId,
                book_id = bookId,
                user_id = userId.ToString(),
                date_due = DateTime.Now.AddDays(1)
            };
            db.Execute("INSERT INTO [Borrows] (id, user_id, book_id, date_due) VALUES " +
                       "(@id, @user_id, @book_id, @date_due)", borrow);
            db.Close();
        }

        private static void UnBorrowBook(int id)
        {
            IDbConnection db = new SqlConnection(connectionString);
            db.Execute("DELETE FROM [Borrows] WHERE " +
                       "book_id = @book_id", new{book_id=id});
            db.Close();
        }

        

        private static string connectionString =
            "Data Source=PEKINGESE\\MSSQLSERVER01;Initial Catalog=master;Integrated Security=True";

        public static List<Book> GetAllBooks()
        {
            IDbConnection db = new SqlConnection(connectionString);
            var books = db.Query("SELECT * FROM [Books]")
                .Select(book => new Book(book.Id, book.author, book.title, book.ISBN))
                .ToList();
            return books;
        }

        public static List<Borrow> GetAllBorrows()
        {
            IDbConnection db = new SqlConnection(connectionString);
            var borrows = db.Query("SELECT * FROM [Borrows]").Select(borrow =>
                new Borrow(borrow.Id, borrow.user_id.ToString(), borrow.book_id, borrow.date_due))
                .ToList();
            return borrows;
        }

        public static List<Book> AvailableBooks()
        {
            IDbConnection db = new SqlConnection(connectionString);
            var borrows = GetAllBorrows();
            var availableBooks = GetAllBooks().Where(book =>
                    borrows.Aggregate(true, (soFar, borrow) => soFar && borrow.book_id != book.id))
                .ToList();
            availableBooks.Sort((book1, book2) => String.Compare(book1.title, book2.title));
            return availableBooks;
        }

        public static List<Book> UserBorrowedBooks(string userId)
        {
            IDbConnection db = new SqlConnection(connectionString);
            var borrows = GetAllBorrows();
            var userBooks = GetAllBooks().Where(book =>
                    borrows.Aggregate(false, (soFar, borrow) => soFar || 
                        (borrow.book_id == book.id && borrow.user_id == userId)))
                .ToList();
            userBooks.Sort((book1, book2) => String.Compare(book1.title, book2.title));
            return userBooks;
        }


    }
}