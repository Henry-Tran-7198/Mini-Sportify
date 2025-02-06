using System.Text;
using BL.Services;
using Persistence.Models;

class Program
{
    private static string GetMaskedInput()
    {
        var password = new StringBuilder();
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Length--;
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return password.ToString();
    }

    private static int ShowMenu(string title, string[] options, string username = "")
    {
        int currentSelection = 0;
        ConsoleKey keyPressed;

        do
        {
            Console.Clear();
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("╔═══════════════════════╗");
                Console.WriteLine($"║     {title,-18}║");
                Console.WriteLine("╚═══════════════════════╝");
            }
            else
            {
                int totalWidth = 24 + username.Length; // Base width (24) + dynamic username length
                int contentLength = title.Length + 3 + username.Length; // title + " - " + username
                int leftPadding = (totalWidth - contentLength) / 2;
                
                string horizontalLine = new string('═', totalWidth);
                string leftSpace = new string(' ', leftPadding);
                
                Console.WriteLine($"╔{horizontalLine}╗");
                Console.WriteLine($"║{leftSpace}{title} - {username}{new string(' ', totalWidth - contentLength - leftPadding)}║");
                Console.WriteLine($"╚{horizontalLine}╝");
            }

            for (int i = 0; i < options.Length; i++)
            {
                if (i == currentSelection)
                    Console.WriteLine($"► {options[i]}");
                else
                    Console.WriteLine($"  {options[i]}");
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.UpArrow)
            {
                currentSelection--;
                if (currentSelection < 0)
                    currentSelection = options.Length - 1;
            }
            else if (keyPressed == ConsoleKey.DownArrow)
            {
                currentSelection++;
                if (currentSelection >= options.Length)
                    currentSelection = 0;
            }

        } while (keyPressed != ConsoleKey.Enter);

        return currentSelection + 1;
    }

    static void Main(string[] args)
    {
        var userService = new UserService();
        bool running = true;

        while(running)
        {
            string[] options = { "Sign In", "Sign Up", "Exit" };
            int choice = ShowMenu("Spotify Clone", options);
            
            switch(choice)
            {
                case 1: SignIn(userService); break;
                case 2: SignUp(userService); break;
                case 3: running = false; break;
            }
        }
    }

    static void SignIn(UserService userService)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════╗");
        Console.WriteLine("║    Sign In     ║");
        Console.WriteLine("╚════════════════╝");
        
        Console.Write("Email: ");
        string? email = Console.ReadLine();
        
        Console.Write("Password: ");
        string? password = GetMaskedInput();

        if(email != null && password != null)
        {
            User? user = userService.SignIn(email, password);
            if(user != null)
            {
                switch(user.Roles.ToLower())
                {
                    case "admin": ShowAdminMenu(user); break;
                    case "artist": ShowArtistMenu(user); break;
                    case "listener": ShowListenerMenu(user); break;
                }
            }
            else
            {
                Console.WriteLine("Invalid credentials!");
                Console.ReadKey();
            }
        }
    }

    static void SignUp(UserService userService)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════╗");
        Console.WriteLine("║    Sign Up     ║");
        Console.WriteLine("╚════════════════╝");

        Console.Write("Username: ");
        string? userName = Console.ReadLine();
        Console.Write("Email: ");
        string? email = Console.ReadLine();
        Console.Write("Password: ");
        string? password = GetMaskedInput();

        string[] roleOptions = { "Listener", "Artist" };
        int roleChoice = ShowMenu("Select Role", roleOptions);
        string role = roleChoice == 1 ? "listener" : "artist";

        if(userName != null && email != null && password != null)
        {
            if(userService.SignUp(userName, email, password, role))
            {
                Console.WriteLine("Sign up successful!");
            }
            else
            {
                Console.WriteLine("Sign up failed!");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void ShowAdminMenu(User user)
    {
        bool running = true;
        string[] options = { "Manage Users", "Manage Content", "Logout" };

        while(running)
        {
            int choice = ShowMenu("Admin Menu", options, user.UserName);
            switch(choice)
            {
                case 3: running = false; break;
                default:
                    Console.WriteLine("Feature coming soon!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ShowArtistMenu(User user)
    {
        bool running = true;
        string[] options = { "Upload Song", "View My Songs", "Logout" };

        while(running)
        {
            int choice = ShowMenu("Artist Menu", options, user.UserName);
            switch(choice)
            {
                case 3: running = false; break;
                default:
                    Console.WriteLine("Feature coming soon!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ShowListenerMenu(User user)
    {
        bool running = true;
        string[] options = { "Browse Songs", "My Playlists", "Logout" };

        while(running)
        {
            int choice = ShowMenu("Listener Menu", options, user.UserName);
            switch(choice)
            {
                case 3: running = false; break;
                default:
                    Console.WriteLine("Feature coming soon!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}