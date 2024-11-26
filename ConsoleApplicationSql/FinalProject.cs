using System.Data.SQLite;
using ConsoleApplicationSql;

namespace Algorithm
{
  public class FinalProject
  {
    BookService bookService = new BookService();
    UserService userService = new UserService();
    BorrowBookService? borrowBookService;
    public void Exec()
    {
      borrowBookService = new BorrowBookService(userService, bookService);
      while (true)
      {
        int choice = DisplayMenu();
        if (choice == 9) break;
        HandleInput(choice);
      }
    }

    public void HandleDatabase(string dbPath)
    {
      SQLiteConnection connection;

      if (!System.IO.File.Exists(dbPath))
      {
        SQLiteConnection.CreateFile(dbPath);
        Console.WriteLine("Database file created.");
      }

      connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
      connection.Open();


      try
      {
        string createUserTableQuery = @"
                CREATE TABLE IF NOT EXISTS User (
                    MembershipId INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL
                );";
        SQLiteCommand createUserTableCmd = new SQLiteCommand(createUserTableQuery, connection);
        createUserTableCmd.ExecuteNonQuery();

        string createBookTableQuery = @"
                CREATE TABLE IF NOT EXISTS Book (
                    ISBN TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Author TEXT NOT NULL,
                    Quantity INTEGER NOT NULL
                );";
        SQLiteCommand createBookTableCmd = new SQLiteCommand(createBookTableQuery, connection);
        createBookTableCmd.ExecuteNonQuery();

        string createBorrowedBookTableQuery = @"
                CREATE TABLE IF NOT EXISTS BorrowedBook (
                    BorrowedBookId INTEGER PRIMARY KEY AUTOINCREMENT,
                    MembershipId INTEGER NOT NULL,
                    ISBN TEXT NOT NULL,
                    DueDate DATE NOT NULL,
                    Quantity INTEGER NOT NULL,
                    FOREIGN KEY (MembershipId) REFERENCES User(MembershipId),
                    FOREIGN KEY (ISBN) REFERENCES Book(ISBN)
                );";
        SQLiteCommand createBorrowedBookTableCmd = new SQLiteCommand(createBorrowedBookTableQuery, connection);
        createBorrowedBookTableCmd.ExecuteNonQuery();

        InsertIfNotExists(connection, "User", "MembershipId", 1,
            "INSERT INTO User (MembershipId, Name) VALUES (@MembershipId, @Name);",
            ("@MembershipId", 1), ("@Name", "John Doe"));
        InsertIfNotExists(connection, "User", "MembershipId", 2,
            "INSERT INTO User (MembershipId, Name) VALUES (@MembershipId, @Name);",
            ("@MembershipId", 2), ("@Name", "Jane Smith"));

        InsertIfNotExists(connection, "Book", "ISBN", "978-0132350884",
            "INSERT INTO Book (ISBN, Title, Author, Quantity) VALUES (@ISBN, @Title, @Author, @Quantity);",
            ("@ISBN", "978-0132350884"), ("@Title", "Clean Code"),
            ("@Author", "Robert C. Martin"), ("@Quantity", 10));
        InsertIfNotExists(connection, "Book", "ISBN", "978-1491950296",
            "INSERT INTO Book (ISBN, Title, Author, Quantity) VALUES (@ISBN, @Title, @Author, @Quantity);",
            ("@ISBN", "978-1491950296"), ("@Title", "Designing Data-Intensive Applications"),
            ("@Author", "Martin Kleppmann"), ("@Quantity", 5));

        InsertIfNotExists(connection, "BorrowedBook", "BorrowedBookId", 1,
            "INSERT INTO BorrowedBook (MembershipId, ISBN, DueDate, Quantity) VALUES (@MembershipId, @ISBN, @DueDate, @Quantity);",
            ("@MembershipId", 1), ("@ISBN", "978-0132350884"),
            ("@DueDate", DateTime.Now.AddDays(14).ToString("yyyy-MM-dd")), ("@Quantity", 1));
        InsertIfNotExists(connection, "BorrowedBook", "BorrowedBookId", 2,
            "INSERT INTO BorrowedBook (MembershipId, ISBN, DueDate, Quantity) VALUES (@MembershipId, @ISBN, @DueDate, @Quantity);",
            ("@MembershipId", 2), ("@ISBN", "978-1491950296"),
            ("@DueDate", DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")), ("@Quantity", 2));
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
      }
      finally
      {
        connection.Close();
      }
    }

    static void InsertIfNotExists(SQLiteConnection connection, string tableName, string keyColumn, object keyValue,
                                 string insertQuery, params (string, object)[] parameters)
    {
      string checkQuery = $"SELECT COUNT(1) FROM {tableName} WHERE {keyColumn} = @KeyValue;";
      SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection);
      checkCmd.Parameters.AddWithValue("@KeyValue", keyValue);

      int count = Convert.ToInt32(checkCmd.ExecuteScalar());
      if (count == 0)
      {
        SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, connection);
        foreach (var param in parameters)
        {
          insertCmd.Parameters.AddWithValue(param.Item1, param.Item2);
        }
        insertCmd.ExecuteNonQuery();
      }
    }

    private int DisplayMenu()
    {
      Console.WriteLine("Welcome to the Library Management System!");
      Console.WriteLine("1. Add a new book:");
      Console.WriteLine("2. Search for book:");
      Console.WriteLine("3. List available books:");
      Console.WriteLine("4. Add a new member:");
      Console.WriteLine("5. Borrow a book:");
      Console.WriteLine("6. Return a book:");
      Console.WriteLine("7. List borrowed books:");
      Console.WriteLine("8. List users: ");
      Console.WriteLine("9. Exit");
      Console.WriteLine();
      Console.Write("Enter your choice: ");
      return int.Parse(Console.ReadLine() ?? "0");
    }

    private void HandleInput(int choice)
    {
      switch (choice)
      {
        case 1:
          Book book = new Book();
          Console.Write("Enter Book Title: ");
          book.Title = Console.ReadLine() ?? "";
          Console.Write("Enter Author: ");
          book.Author = Console.ReadLine() ?? "";
          Console.Write("Enter ISBN: ");
          book.ISBN = Console.ReadLine() ?? "0";
          Console.Write("Enter Quantity: ");
          book.Quantity = int.Parse(Console.ReadLine() ?? "0");
          bookService.AddBook(book);
          break;
        case 2:
          Console.WriteLine("Enter ISBN: ");
          string ISBN = Console.ReadLine() ?? "0";
          bookService.SearchBook(ISBN);
          break;
        case 3:
          bookService.ListBooks();
          break;
        case 4:
          User user = new User();
          Console.Write("Enter Name: ");
          user.Name = Console.ReadLine() ?? "";
          Console.Write("Enter Membership ID: ");
          user.MembershipId = int.Parse(Console.ReadLine() ?? "0");
          userService.AddUser(user);
          break;
        case 5:
          Console.WriteLine("Enter Membership ID: ");
          int membershipId = int.Parse(Console.ReadLine() ?? "0");
          Console.WriteLine("Enter ISBN: ");
          string isbn = Console.ReadLine() ?? "0";
          Console.WriteLine("Enter Quantity: ");
          int quantity = int.Parse(Console.ReadLine() ?? "0");
          if (quantity > 3)
          {
            Console.WriteLine("Cannot borrow more than 3 books!");
            Console.WriteLine();
            return;
          }
          borrowBookService!.BorrowBook(isbn, membershipId, quantity);
          break;
        case 6:
          Console.WriteLine("Enter Membership ID: ");
          membershipId = int.Parse(Console.ReadLine() ?? "0");
          Console.WriteLine("Enter ISBN: ");
          isbn = Console.ReadLine() ?? "0";
          borrowBookService!.ReturnBook(isbn, membershipId);
          break;
        case 7:
          borrowBookService!.ListBorrowedBooks();
          break;
        case 8:
          userService.ListUsers();
          break;
      }
    }
  }
}