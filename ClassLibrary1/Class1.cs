using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dapper;

namespace DataAccess
{
    public class Class1
    {
        
    }

    public class Book
    {
        public readonly int id;
        public readonly string author;
        public readonly string title;
        public readonly string ISBN;
        public string extraInfo;

        public Book(int id, string author, string title, string ISBN)
        {
            this.id = id;
            this.author = author;
            this.title = title;
            this.ISBN = ISBN;
            extraInfo = "";
        }

        public string Describe()
        {
            string description = title + " by " + author;
            if (extraInfo != "")
            {
                description = description + ", " + extraInfo;
            }
            return description;
        } 
    }

    public class User
    {
        public readonly string id;
        public readonly string user_name;
        public readonly string password_hash;

        public User(string id, string user_name, string password_hash)
        {
            this.id = id;
            this.user_name = user_name;
            this.password_hash = password_hash;
        }
    }

    public class Borrow
    {
        public readonly int id;
        public readonly string user_id;
        public readonly int book_id;
        public readonly DateTime date_due;

        public Borrow(int id, string user_id, int book_id, DateTime date_due)
        {
            this.id = id;
            this.user_id = user_id;
            this.book_id = book_id;
            this.date_due = date_due;
        }
    }
}

