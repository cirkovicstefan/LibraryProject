﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces.Repository;
using Common.Models;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class BookRepository : IBookRepository
    {
        public bool AddBook(Book book)
        {
            string query = string.Format("INSERT INTO Books VALUES('{0}','{1}','{2}','{3}','{4}')",
               book.Title, book.ISBN, book.NameAuthor, book.YearOfIssue, book.Quantity);
            return BaseConnection.ExecuteNonQuerySqlCommand(query);

            /*TO BE DONE: On the business logic layer it is necessary to 
             * examine whether there is the same book in the database (in GetAllList()) 
             * if there is a call to the SetQuantity method otherwise Add book*/
        }

        public bool DeleteBook(int IdBook)
        {
            string query = string.Format("DELETE  FROM Books WHERE IdBook='" + IdBook + "'");
            return BaseConnection.ExecuteNonQuerySqlCommand(query);
        }

        public List<Book> GetAllBooks()
        {
            return GetAllBooksInternal("SELECT * FROM Books");
        }
        private int Quantity(Book book)
        {
          //DONE:created  method that returns the quantity from the database based on the forwarded book

            string q = string.Format("SELECT Quantity FROM Books WHERE Title='{0}' AND NameAuthor='{1}'",
                book.Title, book.NameAuthor);
            int quantity = (int)BaseConnection.ExecuteScalarSqlCommand(q);
            if (quantity == 0) return 0;
            return quantity;
        }
        public bool IncrementQuantity(Book book)
        {
            //DONE:created  method that increases the amount of books in database based on the forwarded book
            int quantity = Quantity(book) + 1;
            string query = string.Format("UPDATE Books SET Quantity = '{0}'" +
         " where Title = '{1}' AND NameAuthor='{2}'", quantity, book.Title, book.NameAuthor);
            return BaseConnection.ExecuteNonQuerySqlCommand(query);
        }

        public bool RentedBook(string JmbgUser, Rented rented)
        {
           
            
            if (Quantity(rented) > 0)
            { 
              string query = string.Format("INSERT INTO Rented VALUES('{0}',{1},'{2:u}','{3:u}')",
              JmbgUser, rented.IdBook, rented.DateOfIssue, rented.ReturnDate);
               return BaseConnection.ExecuteNonQuerySqlCommand(query);
            }


            /*TO BE DONE: On the business logic layer, you need to call the RentedBook method, 
             * if it returns true, call the method DecrementQuantity*/
            return false;
        }

        public bool ReturnBook(string JmbgUser, Book book)
        {
            
            string query = "DELETE from Rented where IdBook='" + book.IdBook + "'and JMBGUser='" + JmbgUser + "'";
            return BaseConnection.ExecuteNonQuerySqlCommand(query);

            /*TO BE DONE: Оn the business logic layer it is necessary to call the ReturnBook method,
            if it returns true,  call method  IncrementQuentity  to increase quantity*/


        }

        public List<Book> SearchBook(string search_by, Book book)
        {
            //DONE: created method for search book real time 
            string query = "";
            switch (search_by)
            {
                case "TITLE":
                    query = string.Format("SELECT * FROM Books WHERE Title LIKE '%{0}%'",
                     book.Title);
                    return GetAllBooksInternal(query);
                case "AUTHOR":
                    query = string.Format("SELECT * FROM Books WHERE NameAuthor LIKE '%{0}%'",
                     book.NameAuthor);
                    return GetAllBooksInternal(query);
                default:
                    return new List<Book>();
            }

        }

        public bool SetQuantity(Book book)
        {
            
            int quantity = Quantity(book) + book.Quantity;

            string query = string.Format("UPDATE Books SET Quantity = '{0}'" +
             " WHERE Title = '{1}' AND NameAuthor='{2}'", quantity, book.Title, book.NameAuthor);
            return BaseConnection.ExecuteNonQuerySqlCommand(query);
            
            
        }

        public bool UpdataBook(Book book)
        {
            string query = string.Format("UPDATA Books SET Title='{0}', ISBN='{1}', NameAuthor='{2}'," +
               "YearOfIssue={3}, Quantity={4} where IdBook = {5}", book.Title, book.ISBN, book.NameAuthor, book.YearOfIssue, book.Quantity, book.IdBook);
            return BaseConnection.ExecuteNonQuerySqlCommand(query);
        }
        private List<Book>GetAllBooksInternal(string query)
        {
            List<Book> returnlistbook = new List<Book>();
            using (SqlCommand sqlCommand = BaseConnection.GetSqlCommand(query))
            {
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        Book book = new Book();
                        book.IdBook = sqlDataReader.GetInt32(0);
                        book.Title = sqlDataReader.GetString(1);
                        book.ISBN = sqlDataReader.GetString(2);
                        book.NameAuthor = sqlDataReader.GetString(3);
                        book.YearOfIssue = sqlDataReader.GetInt32(4);
                        book.Quantity = sqlDataReader.GetInt32(5);
                        returnlistbook.Add(book);

                    }
                }

            }
            return returnlistbook;
            
        }

        public bool DecrementQuantity(Book book)
        {
            int quantity = Quantity(book);
            if (quantity > 0)
            {
                int temp = quantity - 1;
                string query = string.Format("UPDATE Books set Quantity = '{0}'" +
             " where Title = '{1}' AND NameAuthor='{2}'", temp, book.Title, book.NameAuthor);
                return BaseConnection.ExecuteNonQuerySqlCommand(query);
            }
            return false;
        }

        public List<Tuple<List<Rented>, List<User>>> GetAllBookRentedUsers()
        {
            List<Rented> listBooksRented = new List<Rented>();
            List<User> listUsers = new List<User>();

           var listtuple = new List<Tuple<List<Rented>, List<User>>>();

            string query = "SELECT u.JMBGUser, u.Name, u.Surname,b.IdBook, b.Title,b.NameAuthor,b.Quantity ,CONVERT(DATE,r.DateOfIssue),CONVERT(DATE,r.ReturnDate)" +
                      " FROM Users u  INNER JOIN Rented r" +
                   " ON(u.JMBGUser = r.JMBGUser)" +
                 " INNER JOIN Books b ON b.IdBook = r.IdBook";


            using (SqlCommand sqlCommand = BaseConnection.GetSqlCommand(query))
            {
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {

                        User user = new User();
                        Rented rented = new Rented();

                        user.JmbgUser = sqlDataReader.GetString(0);
                        user.Name = sqlDataReader.GetString(1);
                        user.Surname = sqlDataReader.GetString(2);
                        rented.IdBook = sqlDataReader.GetInt32(3);
                        rented.Title = sqlDataReader.GetString(4);
                        rented.NameAuthor = sqlDataReader.GetString(5);
                        rented.Quantity = sqlDataReader.GetInt32(6);
                        rented.DateOfIssue = sqlDataReader.GetDateTime(7);
                        rented.ReturnDate = sqlDataReader.GetDateTime(8);

                        listBooksRented.Add(rented);
                        listUsers.Add(user);
                     


                    }
                }
            }
            var t = Tuple.Create(listBooksRented, listUsers);
            listtuple.Add(t);

            return listtuple;


        }
    }
}