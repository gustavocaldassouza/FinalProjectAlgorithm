namespace Algorithm
{
  public class FinalProject
  {
    BookService bookService = new BookService();
    UserService userService = new UserService();
    BorrowService? borrowService;
    public void Exec()
    {
      borrowService = new BorrowService(userService, bookService);
      while (true)
      {
        int choice = DisplayMenu();
        if (choice == 9) break;
        HandleInput(choice);
      }
    }
    int DisplayMenu()
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
    void HandleInput(int choice)
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
          book.ISBN = int.Parse(Console.ReadLine() ?? "0");
          Console.Write("Enter Quantity: ");
          book.Quantity = int.Parse(Console.ReadLine() ?? "0");
          bookService.AddBook(book);
          break;
        case 2:
          Console.WriteLine("Enter ISBN: ");
          int ISBN = int.Parse(Console.ReadLine() ?? "0");
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
          int isbn = int.Parse(Console.ReadLine() ?? "0");
          Console.WriteLine("Enter Quantity: ");
          int quantity = int.Parse(Console.ReadLine() ?? "0");
          if (quantity > 3)
          {
            Console.WriteLine("Cannot borrow more than 3 books!");
            Console.WriteLine();
            return;
          }
          borrowService!.BorrowBook(membershipId, isbn, quantity);
          break;
        case 6:
          Console.WriteLine("Enter Membership ID: ");
          membershipId = int.Parse(Console.ReadLine() ?? "0");
          Console.WriteLine("Enter ISBN: ");
          isbn = int.Parse(Console.ReadLine() ?? "0");
          borrowService!.ReturnBook(isbn, membershipId);
          break;
        case 7:
          borrowService!.ListBorrowedBooks();
          break;
        case 8:
          userService.ListUsers();
          break;
      }
    }
    class BookService
    {
      Book[] books = new Book[2];
      public void AddBook(Book book)
      {
        for (int i = 0; i < books.Length; i++)
        {
          if (books[i].ISBN == book.ISBN)
          {
            Console.WriteLine("Book already exists with the same ISBN!");
            Console.WriteLine("Chose another ISBN!");
            return;
          }

          if (books[i] == null)
          {
            books[i] = book;
            return;
          }
        }

        // initialize a new array with new length and transfer the old one to the new one.
        Book[] newArrBooks = new Book[books.Length + 5];
        for (int i = 0; i < books.Length; i++)
        {
          newArrBooks[i] = books[i];
        }
        newArrBooks[books.Length] = book;
        books = newArrBooks;
      }
      public Book SearchBook(int ISBN)
      {
        foreach (var book in books)
        {
          if (book == null) break;
          if (book.ISBN == ISBN)
          {
            Console.WriteLine("Book found!");
            Console.WriteLine("Title: " + book.Title);
            Console.WriteLine("Author: " + book.Author);
            Console.WriteLine("ISBN: " + book.ISBN);
            Console.WriteLine("Quantity: " + book.Quantity);
            Console.WriteLine();
            return book;
          }
        }
        Console.WriteLine();
        Console.WriteLine("No book found with this specific ISBN!");
        Console.WriteLine();
        return new Book();
      }
      public void ListBooks()
      {
        for (int i = 0; i < books.Length; i++)
        {
          if (books[i] == null) break;
          Console.WriteLine("Title: " + books[i].Title);
          Console.WriteLine("Author: " + books[i].Author);
          Console.WriteLine("ISBN: " + books[i].ISBN);
          Console.WriteLine("Quantity: " + books[i].Quantity);
          Console.WriteLine();
        }
      }
    }
    class Book
    {
      public string? Title { get; set; }
      public string? Author { get; set; }
      public int ISBN { get; set; }
      public int Quantity { get; set; }
    }
    class UserService
    {
      User[] users = new User[2];

      public void AddUser(User user)
      {
        for (int i = 0; i < users.Length; i++)
        {
          if (users[i].MembershipId == user.MembershipId)
          {
            Console.WriteLine("User already exists with this membership ID!");
            Console.WriteLine("Use another membership ID!");
            return;
          }
          if (users[i] == null)
          {
            users[i] = user;
            return;
          }
        }

        // initialize a new array with new length and transfer the old one to the new one.
        User[] newArrUsers = new User[users.Length + 5];
        for (int i = 0; i < users.Length; i++)
        {
          newArrUsers[i] = users[i];
        }
        newArrUsers[users.Length] = user;
        users = newArrUsers;
      }
      public void ListUsers()
      {
        for (int i = 0; i < users.Length; i++)
        {
          if (users[i] == null) break;
          Console.WriteLine("Name: " + users[i].Name);
          Console.WriteLine("Membership ID: " + users[i].MembershipId);
          Console.WriteLine();
        }
      }
      public User SearchUser(int membershipId)
      {
        foreach (var user in users)
        {
          if (user == null) break;
          if (user.MembershipId == membershipId)
          {
            return user;
          }
        }
        return new User();
      }
    }
    class User
    {
      public string? Name { get; set; }
      public int MembershipId { get; set; }
    }
    class BorrowService
    {
      UserService userService;
      BookService bookService;
      BorrowedBook[] borrowedBooks = new BorrowedBook[2];
      public BorrowService(UserService userService, BookService bookService)
      {
        this.userService = userService;
        this.bookService = bookService;
      }
      void AddBorrowedBook(Book book, User user, int quantity)
      {
        BorrowedBook borrowedBook = new BorrowedBook(book, user, quantity);
        for (int i = 0; i < borrowedBooks.Length; i++)
        {
          if (borrowedBooks[i] == null)
          {
            borrowedBooks[i] = borrowedBook;
            return;
          }
        }

        // initialize a new array with new length and transfer the old one to the new one.
        BorrowedBook[] newArrBooks = new BorrowedBook[borrowedBooks.Length + 5];
        for (int i = 0; i < borrowedBooks.Length; i++)
        {
          newArrBooks[i] = borrowedBooks[i];
        }
        newArrBooks[borrowedBooks.Length] = borrowedBook;
        borrowedBooks = newArrBooks;
      }
      public void BorrowBook(int ISBN, int membershipId, int quantity = 1)
      {
        User user = userService.SearchUser(membershipId);
        if (user.MembershipId == membershipId)
        {
          Book book = bookService.SearchBook(ISBN);
          if (book.Quantity > 0)
          {
            book.Quantity--;
            Console.WriteLine("Book borrowed!");
            AddBorrowedBook(book, user, quantity);
            return;
          }
          else
          {
            Console.WriteLine("Book is not available!");
            return;
          }
        }
        Console.WriteLine("User not found!");
      }
      public void ReturnBook(int ISBN, int membershipId)
      {
        User user = userService.SearchUser(membershipId);
        if (user.MembershipId == membershipId)
        {
          Book book = bookService.SearchBook(ISBN);
          // if Book exists
          if (book.ISBN > 0)
          {
            book.Quantity++;
            Console.WriteLine("Book returned!");
            Console.WriteLine();
            for (int i = 0; i < borrowedBooks.Length; i++)
            {
              if (borrowedBooks[i] == null) break;
              if (borrowedBooks[i].book?.ISBN == ISBN && borrowedBooks[i].user?.MembershipId == membershipId)
              {
                if (borrowedBooks[i].dueDate < DateTime.Now)
                {
                  Console.WriteLine("Book is overdue!");
                  Console.WriteLine();
                }
                borrowedBooks[i] = new BorrowedBook();
                return;
              }
            }
            return;
          }
          Console.WriteLine("Book not found!");
          return;
        }
        Console.WriteLine("User not found!");
      }
      public void ListBorrowedBooks()
      {
        bool found = false;
        for (int i = 0; i < borrowedBooks.Length; i++)
        {
          if (borrowedBooks[i] == null) break;
          if (borrowedBooks[i].book == null) continue;
          found = true;
          Console.WriteLine("Title: " + borrowedBooks[i].book?.Title);
          Console.WriteLine("Author: " + borrowedBooks[i].book?.Author);
          Console.WriteLine("ISBN: " + borrowedBooks[i].book?.ISBN);
          Console.WriteLine("Quantity: " + borrowedBooks[i].book?.Quantity);
          Console.WriteLine();
          Console.WriteLine("User: " + borrowedBooks[i].user?.Name);
          Console.WriteLine("Membership ID: " + borrowedBooks[i].user?.MembershipId);
          Console.WriteLine("Due Date: " + borrowedBooks[i].dueDate);
          Console.WriteLine();
          if (borrowedBooks[i].dueDate < DateTime.Now)
          {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Book is overdue!");
            Console.WriteLine();
            Console.ResetColor();
          }
        }
        if (!found)
        {
          Console.WriteLine("No borrowed books found!");
          Console.WriteLine();
        }
      }
    }

    class BorrowedBook
    {
      public Book? book;
      public User? user;
      public int quantity;
      public DateTime dueDate;

      public BorrowedBook(Book book, User user, int quantity)
      {
        this.book = book;
        this.user = user;
        this.quantity = quantity;
        Random random = new Random();
        int days = random.Next(-10, 10);
        this.dueDate = DateTime.Now.AddDays(days);
      }

      public BorrowedBook()
      {

      }
    }
  }
}