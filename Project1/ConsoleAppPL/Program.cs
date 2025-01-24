using System.Text;
using BL;
using Persistence;

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

    static void Main(string[] args)
    {
        UserService userService = new UserService();
        bool running = true;

        while(running)
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════╗");
            Console.WriteLine("║     Spotify Clone     ║");
            Console.WriteLine("╚═══════════════════════╝");
            Console.WriteLine("1. Sign In");
            Console.WriteLine("2. Sign Up");
            Console.WriteLine("3. Exit");
            Console.Write("\nChoose an option: ");

            string? choice = Console.ReadLine();
            switch(choice)
            {
                case "1": SignIn(userService); break;
                case "2": SignUp(userService); break;
                case "3": running = false; break;
                default:
                    Console.WriteLine("Invalid option!");
                    Console.ReadKey();
                    break;
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
        if (email != null && !userService.IsValidEmail(email))
        {
            Console.WriteLine("Invalid email format!");
            Console.ReadKey();
            return;
        }

        Console.Write("Password: ");
        string? password = GetMaskedInput();
        if (password != null && !userService.IsValidPassword(password))
        {
            Console.WriteLine("Password must be at least 8 characters long and contain uppercase, lowercase, and numbers!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nSelect your role:");
        Console.WriteLine("1. Listener");
        Console.WriteLine("2. Artist");
        Console.Write("Choose (1-2): ");
        
        string? roleChoice = Console.ReadLine();
        string? role = roleChoice switch
        {
            "1" => "listener",
            "2" => "artist",
            _ => null
        };

        if(userName != null && email != null && password != null && role != null)
        {
            if(userService.SignUp(userName, email, password, role))
            {
                Console.WriteLine("Sign up successful!");
                Console.WriteLine($"You registered as: {char.ToUpper(role[0]) + role[1..]}");
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
        while(running)
        {
            Console.Clear();
            Console.WriteLine($"╔═════════════════════════════════╗");
            Console.WriteLine($"║ Admin Menu - {user.UserName,-14}║");
            Console.WriteLine($"╚═════════════════════════════════╝");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Content");
            Console.WriteLine("3. Logout");
            Console.Write("\nChoose an option: ");
            
            string? choice = Console.ReadLine();
            switch(choice)
            {
                case "3": running = false; break;
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
        while(running)
        {
            Console.Clear();
            Console.WriteLine($"╔══════════════════════════════════╗");
            Console.WriteLine($"║ Artist Menu - {user.UserName,-13}║");
            Console.WriteLine($"╚══════════════════════════════════╝");
            Console.WriteLine("1. Upload Song");
            Console.WriteLine("2. View My Songs");
            Console.WriteLine("3. Logout");
            Console.Write("\nChoose an option: ");
            
            string? choice = Console.ReadLine();
            switch(choice)
            {
                case "3": running = false; break;
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
        while(running)
        {
            Console.Clear();
            Console.WriteLine($"╔════════════════════════════════════╗");
            Console.WriteLine($"║ Listener Menu - {user.UserName,-12}║");
            Console.WriteLine($"╚════════════════════════════════════╝");
            Console.WriteLine("1. Browse Songs");
            Console.WriteLine("2. My Playlists");
            Console.WriteLine("3. Logout");
            Console.Write("\nChoose an option: ");
            
            string? choice = Console.ReadLine();
            switch(choice)
            {
                case "3": running = false; break;
                default:
                    Console.WriteLine("Feature coming soon!");
                    Console.ReadKey();
                    break;
            }
        }
    }
}