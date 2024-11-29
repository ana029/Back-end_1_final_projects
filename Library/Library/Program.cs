using Library.BookManagerApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Library
{
    namespace BookManagerApp
    {
        public class Book
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public int PublicationYear { get; set; }

            public Book(int id, string title, string author, int publicationYear)
            {
                ID = id;
                Title = title;
                Author = author;
                PublicationYear = publicationYear;
            }

            public override string ToString()
            {
                return $"ID: {ID}, Title: {Title}, Author: {Author}, Year: {PublicationYear}";
            }
        }

        public class BookManager
        {
            private const string FilePath = "books.txt";
            private List<Book> books;

            public BookManager()
            {
                books = LoadBooksFromFile();
            }

            private List<Book> LoadBooksFromFile()
            {
                var bookList = new List<Book>();
                if (File.Exists(FilePath))
                {
                    var lines = File.ReadAllLines(FilePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 4 &&
                            int.TryParse(parts[0], out int id) &&
                            int.TryParse(parts[3], out int publicationYear))
                        {
                            bookList.Add(new Book(id, parts[1], parts[2], publicationYear));
                        }
                    }
                }
                return bookList;
            }

            private void SaveBooksToFile()
            {
                var lines = books.Select(book => $"{book.ID}|{book.Title}|{book.Author}|{book.PublicationYear}");
                File.WriteAllLines(FilePath, lines);
            }

            private int GenerateUniqueID()
            {
                return books.Count > 0 ? books.Max(b => b.ID) + 1 : 1;
            }

            public void AddBook(Book book)
            {
                books.Add(book);
                SaveBooksToFile();
            }

            public void RemoveBookByID(int id)
            {
                var bookToRemove = books.FirstOrDefault(b => b.ID == id);
                if (bookToRemove != null)
                {
                    books.Remove(bookToRemove);
                    SaveBooksToFile();
                    Console.WriteLine($"Book with ID {id} has been removed.");
                }
                else
                {
                    Console.WriteLine($"Book with ID {id} not found.");
                }
            }

            public List<Book> GetAllBooks()
            {
                return new List<Book>(books);
            }

            public Book SearchBookByTitle(string title)
            {
                return books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            }

            public Book CreateBook(string title, string author, int publicationYear)
            {
                int id = GenerateUniqueID();
                return new Book(id, title, author, publicationYear);
            }
        }

        public abstract class User
        {
            public string Name { get; set; }

            public User(string name)
            {
                Name = name;
            }

            public abstract void DisplayRole();
        }

        public class RegisteredUser : User
        {
            private List<Book> cart = new List<Book>();

            public RegisteredUser(string name) : base(name) { }

            public override void DisplayRole()
            {
                Console.WriteLine($"Welcome, Registered User {Name}!");
            }

            public void AddToCart(Book book)
            {
                cart.Add(book);
                Console.WriteLine($"Book '{book.Title}' added to your cart.");
            }

            public void ViewCart()
            {
                if (cart.Count == 0)
                {
                    Console.WriteLine("Your cart is empty.");
                }
                else
                {
                    Console.WriteLine("\n--- Your Cart ---");
                    foreach (var book in cart)
                    {
                        Console.WriteLine(book);
                    }
                }
            }
        }

        public class GuestUser : User
        {
            public GuestUser(string name) : base(name) { }

            public override void DisplayRole()
            {
                Console.WriteLine($"Welcome, Guest User {Name}!");
            }

            public void Register()
            {
                Console.Write("Enter your name: ");
                string name = Console.ReadLine();
                Console.Write("Enter your email: ");
                string email = Console.ReadLine();
                Console.Write("Enter your password: ");
                string password = Console.ReadLine();

                SaveUserData(name, email, password);
                Console.WriteLine("Registration successful! You are now a registered user.");
            }

            private void SaveUserData(string name, string email, string password)
            {
                string fileName = $"{name}_user.txt";
                File.WriteAllLines(fileName, new[] { name, email, password });
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BookManager bookManager = new BookManager();

            Console.Write("Enter your role (Manager/User): ");
            string role = Console.ReadLine()?.ToLower();

            if (role == "manager")
            {
                Console.WriteLine("Welcome, Manager!");
            }
            else if (role == "user")
            {
                User user = GetUserType();
                if (user == null)
                {
                    Console.WriteLine("Exiting program. Please try again later.");
                    return;
                }
                user.DisplayRole();
                if (user is RegisteredUser registeredUser)
                {
                    UserMenu(registeredUser, bookManager);
                }
                else
                {
                    GuestMenu(bookManager);
                }
                return;
            }
            else
            {
                Console.WriteLine("Invalid role. Exiting program.");
                return;
            }

            ManagerMenu(bookManager);
        }

        static void ManagerMenu(BookManager bookManager)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Manager Menu ---");
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. Remove a book by ID");
                Console.WriteLine("3. View all books");
                Console.WriteLine("4. Search for a book by title");
                Console.WriteLine("5. Exit");

                Console.Write("Select an operation: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewBook(bookManager);
                        break;
                    case "2":
                        RemoveBook(bookManager);
                        break;
                    case "3":
                        DisplayAllBooks(bookManager);
                        break;
                    case "4":
                        SearchBook(bookManager);
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void GuestMenu(BookManager bookManager)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Guest Menu ---");
                Console.WriteLine("1. View all books");
                Console.WriteLine("2. Search for a book by title");
                Console.WriteLine("3. Register as a new user");
                Console.WriteLine("4. Exit");

                Console.Write("Select an operation: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllBooks(bookManager);
                        break;
                    case "2":
                        SearchBook(bookManager);
                        break;
                    case "3":
                        GuestUser guest = new GuestUser("Guest");
                        guest.Register();
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void UserMenu(RegisteredUser user, BookManager bookManager)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- User Menu ---");
                Console.WriteLine("1. View all books");
                Console.WriteLine("2. Search for a book by title");
                Console.WriteLine("3. Add a book to cart");
                Console.WriteLine("4. View cart");
                Console.WriteLine("5. Exit");

                Console.Write("Select an operation: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllBooks(bookManager);
                        break;
                    case "2":
                        SearchBook(bookManager);
                        break;
                    case "3":
                        AddBookToCart(user, bookManager);
                        break;
                    case "4":
                        user.ViewCart();
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static User GetUserType()
        {
            Console.Write("Are you a registered user? (yes/no): ");
            string response = Console.ReadLine()?.ToLower();

            if (response == "yes")
            {
                Console.Write("Enter your name: ");
                string name = Console.ReadLine();
                Console.Write("Enter your password: ");
                string password = Console.ReadLine();

                if (ValidateUser(name, password))
                {
                    return new RegisteredUser(name);
                }
                else
                {
                    Console.WriteLine("Invalid name or password. User does not exist.");
                    return null;
                }
            }
            else
            {
                Console.Write("Enter your name: ");
                string name = Console.ReadLine();
                return new GuestUser(name);
            }
        }

        static bool ValidateUser(string name, string password)
        {
            string fileName = $"{name}_user.txt"; 
            if (File.Exists(fileName))
            {
                var userData = File.ReadAllLines(fileName);
                if (userData.Length >= 3 && userData[0] == name && userData[2] == password)
                {
                    return true;
                }
            }
            return false;
        }

        static void AddNewBook(BookManager bookManager)
        {
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();

            Console.Write("Enter book author: ");
            string author = Console.ReadLine();

            Console.Write("Enter publication year: ");
            if (int.TryParse(Console.ReadLine(), out int year))
            {
                var book = bookManager.CreateBook(title, author, year);
                bookManager.AddBook(book);
                Console.WriteLine($"Book '{title}' added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid year. Book not added.");
            }
        }

        static void RemoveBook(BookManager bookManager)
        {
            Console.Write("Enter book ID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bookManager.RemoveBookByID(id);
            }
            else
            {
                Console.WriteLine("Invalid ID. Please try again.");
            }
        }

        static void DisplayAllBooks(BookManager bookManager)
        {
            var books = bookManager.GetAllBooks();
            if (books.Count > 0)
            {
                Console.WriteLine("\n--- Book List ---");
                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("No books available.");
            }
        }

        static void SearchBook(BookManager bookManager)
        {
            Console.Write("Enter the title of the book to search: ");
            string title = Console.ReadLine();
            var book = bookManager.SearchBookByTitle(title);

            if (book != null)
            {
                Console.WriteLine("Book found:");
                Console.WriteLine(book);
            }
            else
            {
                Console.WriteLine($"No book found with the title '{title}'.");
            }
        }

        static void AddBookToCart(RegisteredUser user, BookManager bookManager)
        {
            Console.Write("Enter the title of the book to add to cart: ");
            string title = Console.ReadLine();
            var book = bookManager.SearchBookByTitle(title);

            if (book != null)
            {
                user.AddToCart(book);
            }
            else
            {
                Console.WriteLine($"No book found with the title '{title}'.");
            }
        }

    }
}

