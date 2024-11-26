using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class BookRepository
    {
        private string _dbPath;
        public BookRepository(string dbPath)
        {
            _dbPath = dbPath;
        }
        public void RemoveQuantity(Book book, int quantity)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            string updateQuery = "UPDATE Book SET Quantity = @Quantity WHERE ISBN = @ISBN;";
            SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, connection);
            updateCmd.Parameters.AddWithValue("@Quantity", book.Quantity - quantity);
            updateCmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            updateCmd.ExecuteNonQuery();
            connection.Close();
        }
        public void AddQuantity(Book book, int quantity)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            string updateQuery = "UPDATE Book SET Quantity = @Quantity WHERE ISBN = @ISBN;";
            SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, connection);
            updateCmd.Parameters.AddWithValue("@Quantity", book.Quantity + quantity);
            updateCmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            updateCmd.ExecuteNonQuery();
            connection.Close();
        }
        public void AddBook(Book book)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();

            string insertQuery = "INSERT INTO Book (ISBN, Title, Author, Quantity) VALUES (@ISBN, @Title, @Author, @Quantity);";
            SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            insertCmd.Parameters.AddWithValue("@Title", book.Title);
            insertCmd.Parameters.AddWithValue("@Author", book.Author);
            insertCmd.Parameters.AddWithValue("@Quantity", book.Quantity);
            insertCmd.ExecuteNonQuery();
            connection.Close();
        }
        public Book SearchBook(string ISBN)
        {
            Book book = new Book();
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();

            string selectQuery = "SELECT * FROM Book WHERE ISBN = @ISBN;";
            SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, connection);
            selectCmd.Parameters.AddWithValue("@ISBN", ISBN);
            SQLiteDataReader reader = selectCmd.ExecuteReader();
            while (reader.Read())
            {
                book.ISBN = reader.GetString(0);
                book.Title = reader.GetString(1);
                book.Author = reader.GetString(2);
                book.Quantity = reader.GetInt32(3);
            }
            connection.Close();
            return book;
        }
        public Book[] SearchBooks()
        {
            Book[] books = new Book[1];
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();

            string selectQuery = "SELECT * FROM Book;";
            SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, connection);
            SQLiteDataReader reader = selectCmd.ExecuteReader();
            while (reader.Read())
            {
                Book book = new Book();
                book.ISBN = reader.GetString(0);
                book.Title = reader.GetString(1);
                book.Author = reader.GetString(2);
                book.Quantity = reader.GetInt32(3);
                for (int i = 0; i < books.Length; i++)
                {
                    if (books[i] == null)
                    {
                        books[i] = book;
                        break;
                    }
                }

                if (books[books.Length - 1].ISBN == book.ISBN) continue;
                Array.Resize(ref books, books.Length + 1);
                books[books.Length - 1] = book;
            }

            connection.Close();

            return books;
        }
    }
}