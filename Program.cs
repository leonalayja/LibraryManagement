using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Library library = new Library();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Library Management System");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Register Member");
            Console.WriteLine("3. Register Librarian");
            Console.WriteLine("4. Borrow Book");
            Console.WriteLine("5. Return Book");
            Console.WriteLine("6. Display All Books");
            Console.WriteLine("7. Display All Members");
            Console.WriteLine("8. Display All Librarians");
            Console.WriteLine("9. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            if (string.IsNullOrEmpty(choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
                continue;
            }

            switch (choice)
            {
                case "1":
                    library.AddBook();
                    break;
                case "2":
                    library.RegisterMember();
                    break;
                case "3":
                    library.RegisterLibrarian();
                    break;
                case "4":
                    library.BorrowBook();
                    break;
                case "5":
                    library.ReturnBook();
                    break;
                case "6":
                    library.DisplayAllBooks();
                    break;
                case "7":
                    library.DisplayAllMembers();
                    break;
                case "8":
                    library.DisplayAllLibrarians();
                    break;
                case "9":
                    exit = true;
                    Console.WriteLine("Exiting the system!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }
}

public class Library
{
    private List<Book> books = new List<Book>();
    private List<Member> members = new List<Member>();
    private List<Librarian> librarians = new List<Librarian>();

    public void AddBook()
    {
        Console.Write("Enter Book Title: ");
        string bookTitle = Console.ReadLine() ?? "Unknown Title";

        Console.Write("Enter Author: ");
        string bookAuthor = Console.ReadLine() ?? "Unknown Author";

        Console.Write("Enter ISBN: ");
        string bookISBN = Console.ReadLine() ?? "000000";

        Console.Write("Enter Type (Fiction, Science, History, Technology): ");
        string bookType = Console.ReadLine() ?? "Unknown Type";

        Console.Write("Enter Available Copies: ");
        int availableCopies;
        while (!int.TryParse(Console.ReadLine(), out availableCopies) || availableCopies < 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive number.");
        }

        Librarian? assignedLibrarian = AssignLibrarianToBook(bookType);
        if (assignedLibrarian == null)
        {
            Console.WriteLine($"No librarian found with specialty '{bookType}'. Cannot add the book.");
            return;
        }

        books.Add(new Book(bookTitle, bookAuthor, bookISBN, bookType, availableCopies, assignedLibrarian));
        Console.WriteLine($"Book added successfully and assigned to librarian: {assignedLibrarian.Name} (Specialty: {assignedLibrarian.Specialty})");
    }

    public Librarian? AssignLibrarianToBook(string bookType)
    {
        return librarians.Find(l => l.Specialty.Equals(bookType, StringComparison.OrdinalIgnoreCase));
    }

    public void DisplayAllBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("No books available.");
            return;
        }

        Console.WriteLine("Books in the Library:");
        foreach (var book in books)
        {
            book.DisplayBookInfo();
        }
    }

    public void RegisterMember()
    {
        Console.Write("Enter Member ID: ");
        string memberId = Console.ReadLine() ?? "Unknown ID";

        Console.Write("Enter Member Name: ");
        string memberName = Console.ReadLine() ?? "Unknown Name";

        Console.Write("Enter Membership Type (Basic/Premium): ");
        string membershipType = Console.ReadLine() ?? "Basic";

        members.Add(new Member(memberId, memberName, membershipType));
        Console.WriteLine("Member registered successfully!");
    }

    public void RegisterLibrarian()
    {
        Console.Write("Enter Librarian ID: ");
        string librarianId = Console.ReadLine() ?? "Unknown ID";

        Console.Write("Enter Librarian Name: ");
        string librarianName = Console.ReadLine() ?? "Unknown Name";

        Console.Write("Enter Salary: ");
        double salary;
        while (!double.TryParse(Console.ReadLine(), out salary) || salary < 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive number.");
        }

        Console.Write("Enter Specialty (Fiction, Science, History, Technology): ");
        string specialty = Console.ReadLine() ?? "Unknown Specialty";

        librarians.Add(new Librarian(librarianId, librarianName, salary, specialty));
        Console.WriteLine("Librarian registered successfully!");
    }

    public void BorrowBook()
    {
        Console.Write("Enter Member ID: ");
        string? memberId = Console.ReadLine();

        if (string.IsNullOrEmpty(memberId))
        {
            Console.WriteLine("Invalid Member ID.");
            return;
        }

        Member? member = members.Find(m => m.ID == memberId);
        if (member == null)
        {
            Console.WriteLine("Member not found!");
            return;
        }

        Console.Write("Enter Book ISBN: ");
        string? isbn = Console.ReadLine();

        if (string.IsNullOrEmpty(isbn))
        {
            Console.WriteLine("Invalid ISBN.");
            return;
        }

        Book? book = books.Find(b => b.ISBN == isbn);
        if (book == null)
        {
            Console.WriteLine("Book not found!");
            return;
        }

        if (book.AvailableCopies <= 0)
        {
            Console.WriteLine("No available copies of this book!");
            return;
        }

        member.BorrowBook(book);
    }

    public void ReturnBook()
    {
        Console.Write("Enter Member ID: ");
        string? memberId = Console.ReadLine();

        if (string.IsNullOrEmpty(memberId))
        {
            Console.WriteLine("Invalid Member ID.");
            return;
        }

        Member? member = members.Find(m => m.ID == memberId);
        if (member == null)
        {
            Console.WriteLine("Member not found!");
            return;
        }

        Console.Write("Enter Book ISBN: ");
        string? isbn = Console.ReadLine();

        if (string.IsNullOrEmpty(isbn))
        {
            Console.WriteLine("Invalid ISBN.");
            return;
        }

        Book? book = books.Find(b => b.ISBN == isbn);
        if (book == null)
        {
            Console.WriteLine("Book not found!");
            return;
        }

        member.ReturnBook(book);
    }

    public void DisplayAllMembers()
    {
        if (members.Count == 0)
        {
            Console.WriteLine("No members registered.");
            return;
        }

        Console.WriteLine("Library Members:");
        foreach (var member in members)
        {
            member.DisplayInfo();
        }
    }

    public void DisplayAllLibrarians()
    {
        if (librarians.Count == 0)
        {
            Console.WriteLine("No librarians registered.");
            return;
        }

        Console.WriteLine("Library Librarians:");
        foreach (var librarian in librarians)
        {
            librarian.DisplayInfo();
        }
    }
}

public interface IEntity
{
    string ID { get; }
    string Name { get; }
}

public abstract class PersonBase : IEntity
{
    public string ID { get; private set; }
    public string Name { get; private set; }

    protected PersonBase(string id, string name)
    {
        ID = id;
        Name = name;
    }

    public abstract void DisplayInfo();
}

public class Book
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string ISBN { get; private set; }
    public string Type { get; private set; }
    public int AvailableCopies { get; private set; }
    public Librarian AssignedLibrarian { get; private set; }

    public Book(string title, string author, string isbn, string type, int availableCopies, Librarian assignedLibrarian)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        Type = type;
        AvailableCopies = availableCopies;
        AssignedLibrarian = assignedLibrarian;
    }

    public double GetCost()
    {
        return Type switch
        {
            "Fiction" => 2.0,
            "Science" => 3.0,
            "History" => 2.0,
            "Technology" => 4.0,
            _ => 1.0
        };
    }

    public void BorrowBook()
    {
        if (AvailableCopies > 0)
            AvailableCopies--;
    }

    public void ReturnBook()
    {
        AvailableCopies++;
    }

    public void DisplayBookInfo()
    {
        Console.WriteLine($"Title: {Title}, Author: {Author}, ISBN: {ISBN}, Type: {Type}, Available Copies: {AvailableCopies}, Assigned Librarian: {AssignedLibrarian.Name}");
    }
}

public class Member : PersonBase
{
    public string MembershipType { get; private set; }
    public Dictionary<string, DateTime> BooksBorrowed { get; private set; } = new();
    public double Balance { get; private set; } = 0;

    public Member(string id, string name, string membershipType)
        : base(id, name)
    {
        MembershipType = membershipType;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"ID: {ID}, Name: {Name}, Membership Type: {MembershipType}, Balance: ${Balance}");
    }

    public void BorrowBook(Book book)
    {
        BooksBorrowed[book.ISBN] = DateTime.Now;
        Balance += book.GetCost();
        book.BorrowBook();
        Console.WriteLine($"Book '{book.Title}' has been borrowed. Fee: ${book.GetCost()}");
    }

    public void ReturnBook(Book book)
    {
        if (!BooksBorrowed.ContainsKey(book.ISBN))
        {
            Console.WriteLine("Book not found in borrowed list!");
            return;
        }

        BooksBorrowed.Remove(book.ISBN);
        book.ReturnBook();
        Console.WriteLine($"Book '{book.Title}' has been returned.");
    }
}

public class Librarian : PersonBase
{
    public double Salary { get; private set; }
    public string Specialty { get; private set; }

    public Librarian(string id, string name, double salary, string specialty)
        : base(id, name)
    {
        Salary = salary;
        Specialty = specialty;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"ID: {ID}, Name: {Name}, Salary: {Salary}, Specialty: {Specialty}");
    }
}