using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class BorrowBookRepository
    {
        private string _dbPath;
        public BorrowBookRepository(string dbPath)
        {
            _dbPath = dbPath;
        }
        public void AddBorrowedBook(Book book, User user, int quantity = 1)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            string insertQuery = "INSERT INTO BorrowedBook (ISBN, MembershipId, Quantity, DueDate) VALUES (@ISBN, @MembershipId, @Quantity, @DueDate);";
            SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, connection);
            insertCmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            insertCmd.Parameters.AddWithValue("@MembershipId", user.MembershipId);
            insertCmd.Parameters.AddWithValue("@Quantity", quantity);
            insertCmd.Parameters.AddWithValue("@DueDate", DateTime.Now.AddDays(7));
            insertCmd.ExecuteNonQuery();
            connection.Close();
        }
        public BorrowBook[] ListBorrowedBooks()
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            string selectQuery = "SELECT BorrowedBook.BorrowedBookId, BorrowedBook.ISBN, Book.Title, Book.Author, Book.Quantity, BorrowedBook.MembershipId, User.Name, BorrowedBook.Quantity, BorrowedBook.DueDate FROM BorrowedBook JOIN Book ON BorrowedBook.ISBN = Book.ISBN JOIN User ON BorrowedBook.MembershipId = User.MembershipId;";
            SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, connection);
            SQLiteDataReader reader = selectCmd.ExecuteReader();
            BorrowBook[] borrowedBooks = new BorrowBook[1];
            while (reader.Read())
            {
                Book book = new Book();
                book.ISBN = reader.GetString(1);
                book.Title = reader.GetString(2);
                book.Author = reader.GetString(3);
                book.Quantity = reader.GetInt32(4);
                User user = new User();
                user.MembershipId = reader.GetInt32(5);
                user.Name = reader.GetString(6);
                BorrowBook borrowBook = new BorrowBook(reader.GetInt32(0), book, user, reader.GetInt32(7), reader.GetDateTime(8));
                for (int i = 0; i < borrowedBooks.Length; i++)
                {
                    if (borrowedBooks[i] == null)
                    {
                        borrowedBooks[i] = borrowBook;
                        break;
                    }
                }

                if (borrowedBooks[borrowedBooks.Length - 1].Id == borrowBook.Id) continue;
                Array.Resize(ref borrowedBooks, borrowedBooks.Length + 1);
                borrowedBooks[borrowedBooks.Length - 1] = borrowBook;
            }

            connection.Close();

            return borrowedBooks;
        }
        public void ReturnBook(Book book, User user)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            string deleteQuery = "DELETE FROM BorrowedBook WHERE ISBN = @ISBN AND MembershipId = @MembershipId;";
            SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, connection);
            deleteCmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            deleteCmd.Parameters.AddWithValue("@MembershipId", user.MembershipId);
            deleteCmd.ExecuteNonQuery();
            connection.Close();
        }
        public int FetchQuantityBorrowed(Book book)
        {
            int quantity = 0;
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();
            string selectQuery = "SELECT Quantity FROM BorrowedBook WHERE ISBN = @ISBN;";
            SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, connection);
            selectCmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            SQLiteDataReader reader = selectCmd.ExecuteReader();
            while (reader.Read())
            {
                quantity = reader.GetInt32(0);
            }
            connection.Close();

            return quantity;
        }
    }
}