# **Library Management System**

## **Overview**

This is a console-based Library Management System built in C#. The system allows users to manage books, members, and borrowing operations.

## **Features**

- Add new books to the library
- Search for books by ISBN
- List available books
- Add new members to the library
- Borrow books
- Return books
- List borrowed books
- List all members

## **Database**

The system uses a SQLite database to store data. The database is created automatically when the system is run for the first time.

## **Classes and Methods**

- `FinalProject`: The main class that handles the execution of the system.
  - `HandleDatabase`: Creates the database and tables if they do not exist.
  - `Exec`: Runs the main loop of the system, displaying the menu and handling user input.
- `BookService`: Handles book-related operations.
  - `AddBook`: Adds a new book to the library.
  - `SearchBook`: Searches for a book by ISBN.
  - `ListBooks`: Lists all available books.
- `UserService`: Handles member-related operations.
  - `AddUser`: Adds a new member to the library.
  - `SearchUser`: Searches for a member by membership ID.
  - `ListUsers`: Lists all members.
- `BorrowBookService`: Handles borrowing operations.
  - `BorrowBook`: Borrows a book.
  - `ReturnBook`: Returns a book.
  - `ListBorrowedBooks`: Lists all borrowed books.

## **Usage**

1.  Run the system by executing the `Program.cs` file.
2.  Follow the menu prompts to perform operations.

## **Notes**

- The system uses a simple console-based interface.
- The database is stored in a file named `myDatabase.sqlite` in the same directory as the executable.
- The system does not handle errors robustly and is intended for demonstration purposes only.
