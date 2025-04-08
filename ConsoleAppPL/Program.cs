using System.Text;
using BL;
using BL.Services;
using Persistence;
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
                int totalWidth = 24 + username.Length;
                int contentLength = title.Length + 3 + username.Length;
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

    private static ArtistService artistService = new ArtistService();
    private static SongService songService = new SongService();

    static void Main(string[] args)
    {
        var userService = new UserService();
        bool running = true;

        while(running)
        {
            string[] options = { "Sign In", "Sign Up", "Exit" };
            int choice = ShowMenu("Mini Spotify", options);

            switch(choice)
            {
                case 1: SignIn(userService); break;
                case 2: SignUp(userService); break;
                case 3: running = false; break;
            }
        }
        ShowManageArtistsMenu(artistService);
        ShowManageSongsMenu(songService);
    }

    static void SignIn(UserService userService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════╗");
            Console.WriteLine("║    Sign In     ║");
            Console.WriteLine("╚════════════════╝");
            
            Console.Write("Email: ");
            string? email = "";
            
            // Read email with Escape support
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                    return;
                if (key.Key == ConsoleKey.Enter)
                    break;
                if (key.Key == ConsoleKey.Backspace && email.Length > 0)
                {
                    email = email[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    email += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();

            Console.Write("Password: ");
            string? password = "";
            
            // Read password with Escape support
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                    return;
                if (key.Key == ConsoleKey.Enter)
                    break;
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                User? user = userService.SignIn(email, password);
                if (user != null)
                {
                    switch(user.Roles.ToLower())
                    {
                        case "admin": ShowAdminMenu(user, userService); break;
                        case "artist": ShowArtistMenu(user); break;
                        case "listener": ShowListenerMenu(user); break;
                    }
                    return;
                }
                else
                {
                    Console.WriteLine("\nInvalid credentials!");
                    Console.WriteLine("Press any key to try again or Esc to go back...");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        return;
                }
            }
        }
    }

    static void SignUp(UserService userService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════╗");
            Console.WriteLine("║    Sign Up     ║");
            Console.WriteLine("╚════════════════╝");

            
            // Username input
            string userName = "";
            while (true)
            {
                Console.Write("Username: ");
                userName = "";

                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    return;
                    if (key.Key == ConsoleKey.Enter && userName.Length > 0)
                    break;
                    if (key.Key == ConsoleKey.Backspace && userName.Length > 0)
                    {
                        userName = userName[..^1];
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        userName += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }
                if (userService.CheckUserNameExists(userName))
                {
                    Console.Write("\r"); // Di chuyển con trỏ về đầu dòng
                    Console.Write(new string(' ', Console.WindowWidth)); // Xóa dòng bằng cách ghi đè bằng dấu cách
                    Console.Write("\r"); // Di chuyển lại về đầu dòng lần nữa
                    Console.WriteLine("That Name is already taken. Please choose another one.");
                    continue;
                }
                else
                {
                    break; 
                }
            }
            Console.WriteLine();


            // Email input  
            string email = "";
            while (true)
            {
                Console.Write("Email: ");
                email = "";
                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                        return;
                    if (key.Key == ConsoleKey.Enter && email.Length > 0)
                        break;
                    if (key.Key == ConsoleKey.Backspace && email.Length > 0)
                    {
                        email = email[..^1];
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        email += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }
                if (userService.CheckUserEmailExists(email))
                {
                    Console.Write("\r"); // Di chuyển con trỏ về đầu dòng
                    Console.Write(new string(' ', Console.WindowWidth)); // Xóa dòng bằng cách ghi đè bằng dấu cách
                    Console.Write("\r"); // Di chuyển lại về đầu dòng lần nữa
                    Console.WriteLine("That Email is already taken. Please choose another one.");
                    continue;
                }
                else 
                {
                    break;
                }
            }
            Console.WriteLine();

            // Password input
            Console.Write("Password: ");
            string password = "";
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                    return;
                if (key.Key == ConsoleKey.Enter && password.Length > 0)
                    break;
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();

            // Role selection
            Console.WriteLine("\nSelect Role:");
            string[] roleOptions = { "Listener", "Artist" };
            int currentSelection = 0;
            ConsoleKey roleKey;

            do
            {
                Console.SetCursorPosition(0, Console.CursorTop - roleOptions.Length);
                for (int i = 0; i < roleOptions.Length; i++)
                {
                    if (i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine($"{(i == currentSelection ? "► " : "  ")}{roleOptions[i]}");

                    Console.ResetColor();
                }

                roleKey = Console.ReadKey(true).Key;

                switch (roleKey)
                {
                    case ConsoleKey.UpArrow:
                    if (currentSelection > 0) currentSelection--;
                        break;
                    case ConsoleKey.DownArrow:
                    if (currentSelection < roleOptions.Length - 1) currentSelection++;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            } while (roleKey != ConsoleKey.Enter);

            string role = currentSelection == 0 ? "listener" : "artist";

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                if (userService.SignUp(userName, email, password, role))
                {
                    Console.WriteLine("\nSign up successful!");
                }
                else
                {
                    Console.WriteLine("\nSign up failed! Email might already be in use.");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
                return;
            }
        }
    }
    
    static void ShowAdminMenu(User user, UserService userService)
    {
        bool running = true;
        string[] options = { "Manage Users", "Manage Artists", "Manage Songs", "Logout" };

        while(running)
        {
            int choice = ShowMenu("Admin Menu", options, user.UserName);
            switch(choice)
            {
                case 1:
                ShowManageUsersMenu(userService);
                break;

                case 2:
                ShowManageArtistsMenu(artistService);
                break;

                case 3:
                ShowManageSongsMenu(songService);
                break;

                case 4: running = false; break;
                default:
                    Console.WriteLine("Feature coming soon!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ShowManageUsersMenu(UserService userService)
    {
        bool running = true;
        string[] userOptions = { "Search User", "Delete User", "Show All Users", "Back to Admin Menu" };

        while (running)
        {
            int choice = ShowMenu("Manage Users", userOptions, "Admin");

            switch (choice)
            {
                case 1: // 🔎 Search User
                    Console.Clear();
                    Console.Write("🔍 Enter User ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int userId))
                    {
                        Console.WriteLine("❌ Invalid ID! Please enter a number.");
                        Console.ReadKey();
                        break;
                    }

                    var user = userService.SearchUser(userId); // Gọi tìm kiếm
                    if (user != null)
                    {
                        ShowUserDetails(user); // ✅ Gọi hàm hiển thị thông tin user
                    }
                    else
                    {
                        Console.WriteLine("❌ No user found.");
                    }

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    break;

                case 2: // 🗑 Delete User
                    Console.Clear();
                    Console.Write("🗑 Enter User ID to delete: ");

                    if (!int.TryParse(Console.ReadLine(), out userId))
                    {
                        Console.WriteLine("❌ Invalid ID! Please enter a number.");
                        Console.ReadKey();
                        break;
                    }

                    user = userService.SearchUser(userId); // Tìm user để xóa
                    if (user == null)
                    {
                        Console.WriteLine("❌ No user found with that ID.");
                        Console.ReadKey();
                        break;
                    }

                    ShowUserDetails(user); // ✅ Hiển thị thông tin trước khi xác nhận

                    // Yêu cầu xác nhận xóa
                    Console.Write("❓ Are you sure you want to delete this user? (Y/N): ");
                    string confirm = Console.ReadLine()?.Trim().ToLower();

                    if (confirm != "y")
                    {
                        Console.WriteLine("🚫 Delete canceled.");
                        Console.ReadKey();
                        break;
                    }

                    bool result = userService.DeleteUser(userId);
                    if (result)
                    {
                        Console.WriteLine("\n✅ User deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("\n❌ Failed to delete user.");
                    }

                    Console.ReadKey();
                    break;

                case 3:
                    ShowAllUsers(userService);
                    break;

                case 4:
                    running = false;
                    break;

                default:
                    Console.WriteLine("⚠️ Invalid choice. Try again!");
                    Console.ReadKey();
                    break;
            }
        }
    }
    
    static void ShowUserDetails(User user)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║               🔎 User Found              ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        string header = "╔══════════╦══════════════════════╦══════════════════════╦══════════╗";
        string footer = "╚══════════╩══════════════════════╩══════════════════════╩══════════╝";
    
        Console.WriteLine(header);
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-8} ║", 
                "User ID", "User Name", "Email", "Role");
        Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬══════════╣");
    
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-8} ║",
                user.UserId,
                Truncate(user.UserName, 20),
                Truncate(user.UserEmail, 20),
                Truncate(user.Roles, 8));
    
        Console.WriteLine(footer);
    }

    static void ShowAllUsers(UserService userService)
    {
        Console.Clear();
        var users = userService.GetAllUsers();
        if (users.Count == 0)
        {
            Console.WriteLine("❌ No users found.");
            return;
        }

        int currentPage = 0;
        int itemsPerPage = 10;
        int totalPages = (int)Math.Ceiling(users.Count / (double)itemsPerPage);
        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║             📜 All Users                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            // Hiển thị bảng
            string header = "╔══════════╦══════════════════════╦══════════════════════╦══════════╗";
            string footer = "╚══════════╩══════════════════════╩══════════════════════╩══════════╝";

            Console.WriteLine(header);
            Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-8} ║",
                        "User ID", "User Name", "Email", "Role");
            Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬══════════╣");

            var pageUser = users.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
            foreach (var user in pageUser)
            {
                Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-8} ║",
                                   user.UserId,
                                   Truncate(user.UserName, 20),
                                   Truncate(user.UserEmail, 20),
                                   Truncate(user.Roles, 8));
            }
            Console.WriteLine(footer);
            Console.WriteLine($"\nPage {currentPage + 1}/{totalPages}");
            Console.WriteLine("Use ← → to change pages, Esc to exit");

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (currentPage > 0)
                    {
                        currentPage--;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (currentPage < totalPages - 1)
                    {
                        currentPage++;
                    }
                    break;
            }
        } while (key != ConsoleKey.Escape);
    }

    static void ShowManageArtistsMenu(ArtistService artistService)
    {
        bool running = true;
        string[] userOptions = { "Search Artist", "Delete Artist", "Show All Artists", "Back to Admin Menu" };

        while (running)
        {
            int choice = ShowMenu("Manage Artists", userOptions, "Admin");
            switch (choice)
            {
                case 1:
                    SearchArtistById(artistService);
                    break;
                
                case 2:
                    Console.Clear();
                    Console.Write("🗑 Enter Artist ID to delete: ");
                    int artistId;

                    if (!int.TryParse(Console.ReadLine(), out artistId))
                    {
                        Console.WriteLine("❌ Invalid ID! Please enter a number.");
                        Console.ReadKey();
                        break;
                    }

                    // Gọi hàm tìm kiếm thông tin nghệ sĩ
                    var artist = artistService.SearchArtist(artistId);

                    if (artist == null)
                    {
                        Console.WriteLine("❌ No artist found with that ID.");
                        Console.ReadKey();
                        break;
                    }

                    // Hiển thị thông tin nghệ sĩ
                    Console.Clear();
                    Console.WriteLine("╔══════════════════════════════════════════╗");
                    Console.WriteLine("║            🎵 Artist Information         ║");
                    Console.WriteLine("╚══════════════════════════════════════════╝");

                    string header = "╔═══════════╦══════════════════════╦══════════════════════╦════════════╗";
                    string footer = "╚═══════════╩══════════════════════╩══════════════════════╩════════════╝";

                    Console.WriteLine(header);
                    Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", 
                    "Artist ID", "Artist Name", "Top Song", "Birth Date");
                    Console.WriteLine("╠═══════════╬══════════════════════╬══════════════════════╬════════════╣");

                    Console.WriteLine("║ {0,-9} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                        artist.ArtistId,
                        Truncate(artist.ArtistName, 20),
                        Truncate(artist.TopSong, 20),
                        artist.BirthDate?.ToString("yyyy-MM-dd") ?? "N/A");

                    Console.WriteLine(footer);
    
                    // Yêu cầu xác nhận xóa
                    Console.Write("❓ Are you sure you want to delete this artist? (Y/N): ");
                    string confirm = Console.ReadLine()?.Trim().ToLower();

                    if (confirm != "y")
                    {
                        Console.WriteLine("🚫 Delete canceled.");
                        Console.ReadKey();
                        return;
                    }

                    // Thực hiện xóa nghệ sĩ
                    bool result = artistService.DeleteArtist(artistId);
                    if (result)
                    {
                        Console.WriteLine("\n✅ Artist deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("\n❌ Failed to delete artist.");
                    }

                    Console.ReadKey();
                    break;

                case 3:
                    ShowAllArtists(artistService);
                    break;

                case 4:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void SearchArtistById(ArtistService artistService)
    {
        Console.Clear();
        Console.Write("🔍 Enter Artist ID: ");

        if (!int.TryParse(Console.ReadLine(), out int artistId))
        {
            Console.WriteLine("❌ Invalid ID! Please enter a number.");
            Console.ReadKey();
            return;
        }

        var artist = artistService.SearchArtist(artistId); // Gọi hàm tìm kiếm từ ArtistService

        if (artist != null)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║              🔎 Artist Found             ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            string header = "╔═══════════╦══════════════════════╦══════════════════════╦════════════╗";
            string footer = "╚═══════════╩══════════════════════╩══════════════════════╩════════════╝";

            Console.WriteLine(header);
            Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", 
                    "Artist ID", "Artist Name", "Top Song", "Birth Date");
            Console.WriteLine("╠═══════════╬══════════════════════╬══════════════════════╬════════════╣");

            Console.WriteLine("║ {0,-9} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                    artist.ArtistId,
                    Truncate(artist.ArtistName, 20),
                    Truncate(artist.TopSong, 20),
                    artist.BirthDate?.ToString("yyyy-MM-dd") ?? "N/A");

            Console.WriteLine(footer);
        }
        else
        {
            Console.WriteLine("❌ No artist found.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void ShowAllArtists(ArtistService artistService)
    {
        Console.Clear();
        var artists = artistService.GetAllArtists();
        if (artists.Count == 0)
        {
            Console.WriteLine("❌ No artists found.");
            return;
        }

        int currentPage = 0;
        int itemsPerPage = 10;
        int totalPages = (int)Math.Ceiling(artists.Count / (double)itemsPerPage);
        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║             📜 All Artists               ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            // Hiển thị bảng
            string header = "╔═══════════╦══════════════════════╦══════════════════════╦════════════╗";
            string footer = "╚═══════════╩══════════════════════╩══════════════════════╩════════════╝";

            Console.WriteLine(header);
            Console.WriteLine("║ {0,-9} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", 
            "Artist ID", "Artist Name", "Top Song", "Birth Date");
            var pageArtist = artists.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
            foreach (var artist in pageArtist)
            {
                Console.WriteLine("║ {0,-9} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                                    artist.ArtistId,
                                    Truncate(artist.ArtistName, 20),
                                    Truncate(artist.TopSong, 20),
                                    artist.BirthDate?.ToString("yyyy-MM-dd") ?? "N/A");
            }
            Console.WriteLine(footer);
            Console.WriteLine($"\nPage {currentPage + 1}/{totalPages}");
            Console.WriteLine("Use ← → to change pages, Esc to exit");

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (currentPage > 0)
                    {
                        currentPage--;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (currentPage < totalPages - 1)
                    {
                        currentPage++;
                    }
                    break;
            }
        } while (key != ConsoleKey.Escape);
    }
    
    static void ShowManageSongsMenu(SongService songService)
    {
        bool running = true;
        string[] userOptions = { "Search Song", "Delete Song", "Show All Songs", "Back to Admin Menu" };

        while (running)
        {
            int choice = ShowMenu("Manage Songs", userOptions, "Admin");
            switch (choice)
            {
                case 1:
                    SearchSongById(songService);
                    break;
                    
                case 2:
                    DeleteSongById(songService);
                    break;


                case 3:
                    ShowAllSongs(songService);
                    break;

                case 4:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void SearchSongById(SongService songService)
    {
        Console.Clear();
        Console.Write("🔍 Enter Song ID: ");

        if (!int.TryParse(Console.ReadLine(), out int songId))  // Sửa tên biến
        {
            Console.WriteLine("❌ Invalid ID! Please enter a number.");
            Console.ReadKey();
            return;
        }

        var song = songService?.SearchSong(songId);  // Kiểm tra null
        if (song == null)
        {
            Console.WriteLine("❌ No song found.");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║              🔎 Song Found               ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        string header = "╔══════════╦══════════════════════╦══════════════════════╦════════════╗";
        string footer = "╚══════════╩══════════════════════╩══════════════════════╩════════════╝";

        Console.WriteLine(header);
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", "Song ID", "Title", "Album", "Genre");
        Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬════════════╣");

        Console.WriteLine("║ {0,-9}║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
            song.SongId,
            Truncate(song.Title, 20),
            Truncate(song.Album, 20),
            Truncate(song.Genre, 10));

        Console.WriteLine(footer);

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void DeleteSongById(SongService songService)
    {
        Console.Clear();
        Console.Write("🗑 Enter Song ID to delete: ");
        int songId;

        if (!int.TryParse(Console.ReadLine(), out songId))
        {
            Console.WriteLine("❌ Invalid ID! Please enter a number.");
            Console.ReadKey();
            return;
        }

        // Gọi hàm tìm kiếm thông tin song
        var song = songService.SearchSong(songId);

        if (song == null)
        {
            Console.WriteLine("❌ No song found with that ID.");
            Console.ReadKey();
            return;
        }

        // Hiển thị thông tin song
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║            🎵 Song Information           ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        string header = "╔══════════╦══════════════════════╦══════════════════════╦════════════╗";
        string footer = "╚══════════╩══════════════════════╩══════════════════════╩════════════╝";

        Console.WriteLine(header);
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", "Song ID", "Title", "Album", "Genre");
        Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬════════════╣");

        Console.WriteLine("║ {0,-9}║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
            song.SongId,
            Truncate(song.Title, 20),
            Truncate(song.Album, 20),
            Truncate(song.Genre, 10));

        Console.WriteLine(footer);
    
        // Yêu cầu xác nhận xóa
        Console.Write("❓ Are you sure you want to delete this song? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToLower();

        if (confirm != "y")
        {
            Console.WriteLine("🚫 Delete canceled.");
            Console.ReadKey();
            return;
        }

        // Thực hiện xóa song
        bool result = songService.DeleteSong(songId, song.ArtistId);
        if (result)
        {
            Console.WriteLine("\n✅ Song deleted successfully.");
        }
        else
        {
            Console.WriteLine("\n❌ Failed to delete song.");
        }

        Console.ReadKey();
        return;
    }
    
    static void ShowAllSongs(SongService songService)
    {
        Console.Clear();
        var songs = songService.GetAllSongs();
        if (songs.Count == 0)
        {
            Console.WriteLine("❌ No songs found.");
            return;
        }

        int currentPage = 0;
        int itemsPerPage = 10;
        int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║             📜 All Songs                 ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            // Định dạng bảng
            string header = "╔══════════╦══════════════════════╦══════════════════════╦════════════╗";
            string footer = "╚══════════╩══════════════════════╩══════════════════════╩════════════╝";

            Console.WriteLine(header);
            Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", "Song ID", "Title", "Album", "Genre");
            Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬════════════╣");
            var pageSong = songs.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
            foreach (var song in pageSong)
            {
                Console.WriteLine("║ {0,-9}║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                                song.SongId,
                                Truncate(song.Title, 20),
                                Truncate(song.Album, 20),
                                Truncate(song.Genre, 10));
            }
            Console.WriteLine(footer);
            Console.WriteLine($"\nPage {currentPage + 1}/{totalPages}");
            Console.WriteLine("Use ← → to change pages, Esc to exit");

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (currentPage > 0)
                    {
                        currentPage--;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (currentPage < totalPages - 1)
                    {
                        currentPage++;
                    }
                    break;
            }
        } while (key != ConsoleKey.Escape);
    }
   

    static void ShowArtistMenu(User user)
    {
        bool running = true;
        string[] options = { 
            "Register Information",
            "Upload Song",
            "Delete Song", 
            "View My Songs",
            "Logout"
        };

        ArtistService artistService = new ArtistService();
        SongService songService = new SongService();

        while(running)
        {
            int choice = ShowMenu("Artist Menu", options, user.UserName);
            switch(choice)
            {
                case 1:
                    RegisterArtist(artistService, user);
                    break;
                case 2:
                    UploadSong(songService);
                    break;
                case 3:
                    DeleteSong(songService, artistService, user);
                    break;
                case 4:
                    ViewMySongs(songService, artistService, user);
                    break;
                case 5: 
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    Console.ReadKey();
                    break;
            }
        }
    }

// Add this new method for viewing songs
    static void ViewMySongs(SongService songService, ArtistService artistService, User currentUser)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║                   My Songs               ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        
        var artist = artistService.GetArtistByUserId(currentUser.UserId);
        if (artist == null)
        {
            Console.WriteLine("Artist information not found!");
            Console.ReadKey();
            return;
        }

        var songs = songService.GetSongsByArtist(artist.ArtistId);
        if (!songs.Any())
        {
            Console.WriteLine("You have no songs yet!");
            Console.ReadKey();
            return;
        }

        int currentSelection = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║                   My Songs               ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            // Định dạng bảng
            string header = "╔══════════╦══════════════════════╦══════════════════════╦════════════╗";
            string footer = "╚══════════╩══════════════════════╩══════════════════════╩════════════╝";

            Console.WriteLine(header);
            Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", "Song ID", "Title", "Album", "Genre");
            Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬════════════╣");

            for (int i = 0; i < songs.Count; i++)
            {
                var song = songs[i];
                if (i == currentSelection)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("║ {0,-9}║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                                songs[i].SongId.ToString().PadRight(9),
                                Truncate(songs[i].Title, 20),
                                Truncate(songs[i].Album, 20),
                                Truncate(songs[i].Genre, 10));
                
                Console.ResetColor();
            }

            Console.WriteLine(footer);
            Console.WriteLine("\nUse ↑↓ to navigate, Esc to go back");

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (currentSelection > 0) currentSelection--;
                    break;
                case ConsoleKey.DownArrow:
                    if (currentSelection < songs.Count - 1) currentSelection++;
                    break;
                case ConsoleKey.Escape:
                    return;
            }

        } while (key != ConsoleKey.Enter);
        Console.ReadKey();
    }

    static void RegisterArtist(ArtistService artistService, User currentUser)
    {
        if (artistService.IsArtistRegistered(currentUser.UserId))
        {
            Console.WriteLine("\nYou have already registered as an artist!");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║             Artist Registration          ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        string? name;
        do
        {
            Console.Write("Enter artist name: ");
            name = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(name));

        DateTime birthDate;
        while (true)
        {
            Console.Write("Enter birth date (YYYY-MM-DD): ");
            string? input = Console.ReadLine();
            if (DateTime.TryParse(input, out birthDate))
            {
                break;
            }
            Console.WriteLine("Invalid date format. Please try again.");
        }

        string? topSong;
        do
        {
            Console.Write("Enter your top song: ");
            topSong = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(topSong));

        if (artistService.RegisterArtist(name, birthDate, topSong, currentUser.UserId))
        {
            Console.WriteLine("\nArtist registered successfully!");
        }
        else
        {
            Console.WriteLine("\nFailed to register artist.");
        }
        Console.ReadKey();
    }

    static void UploadSong(SongService songService)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║                  Upload Song             ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        // Title input
        string title = "";
        while (true)
        {
            Console.Write("Enter title of song: ");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(title)) break;
                }
                else if (key.Key == ConsoleKey.Backspace && title.Length > 0)
                {
                    title = title[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    title += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(title)) break;
        }

        // Artist name input
        string artistName = "";
        while (true)
        {
            Console.Write("Enter artist name: ");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(artistName)) break;
                }
                else if (key.Key == ConsoleKey.Backspace && artistName.Length > 0)
                {
                    artistName = artistName[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    artistName += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(artistName)) break;
        }

        // Album input
        string album = "";
        while (true)
        {
            Console.Write("Enter album of song: ");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(album)) break;
                }
                else if (key.Key == ConsoleKey.Backspace && album.Length > 0)
                {
                    album = album[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    album += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(album)) break;
        }

        // Genre input
        string genre = "";
        while (true)
        {
            Console.Write("Enter genre of song: ");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(genre)) break;
                }
                else if (key.Key == ConsoleKey.Backspace && genre.Length > 0)
                {
                    genre = genre[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    genre += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(genre)) break;
        }

        // Release date input
        DateTime releaseDate;
        while (true)
        {
            Console.Write("Enter release date (YYYY-MM-DD): ");
            string input = "";
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(input)) break;
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            
            if (DateTime.TryParse(input, out releaseDate))
                break;
            
            Console.WriteLine("Invalid date format. Please enter again.");
        }

        if (songService.UploadSong(title, artistName, album, genre, releaseDate))
        {
            Console.WriteLine("\nSong uploaded successfully!");
        }
        else
        {
            Console.WriteLine("\nFailed to upload song. Artist not found!");
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void DeleteSong(SongService songService, ArtistService artistService, User currentUser)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║               Delete Song                ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        Console.WriteLine("Press Esc at any time to go back\n");
        
        // Get artist ID for current user
        var artist = artistService.GetArtistByUserId(currentUser.UserId);
        if (artist == null)
        {
            Console.WriteLine("Artist information not found!");
            Console.ReadKey();
            return;
        }

        // Get and display all songs by this artist
        var songs = songService.GetSongsByArtist(artist.ArtistId);
        if (!songs.Any())
        {
            Console.WriteLine("You have no songs to delete!");
            Console.ReadKey();
            return;
        }

        int currentSelection = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║               Delete Song                ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");

            // Định dạng bảng
            string header = "╔══════════╦══════════════════════╦══════════════════════╦════════════╗";
            string footer = "╚══════════╩══════════════════════╩══════════════════════╩════════════╝";

            Console.WriteLine(header);
            Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", "Song ID", "Title", "Album", "Genre");
            Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬════════════╣");

            for (int i = 0; i < songs.Count; i++)
            {
                var song = songs[i];
                if (i == currentSelection)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("║ {0,-9}║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                                songs[i].SongId.ToString().PadRight(9),
                                Truncate(songs[i].Title, 20),
                                Truncate(songs[i].Album, 20),
                                Truncate(songs[i].Genre, 10));

                Console.ResetColor();
            }

            Console.WriteLine(footer);
            Console.WriteLine("\nUse ↑↓ to navigate, Enter to select, Esc to cancel");

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (currentSelection > 0) currentSelection--;
                    break;
                case ConsoleKey.DownArrow:
                    if (currentSelection < songs.Count - 1) currentSelection++;
                    break;
                case ConsoleKey.Escape:
                    return;
            }

        } while (key != ConsoleKey.Enter);

        // Confirm deletion
        var selectedSong = songs[currentSelection];
        Console.Clear();
        Console.WriteLine($"\nAre you sure you want to delete '{selectedSong.Title}'?");
        Console.WriteLine("Press Enter to confirm, any other key to cancel...");

        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            if (songService.DeleteSong(selectedSong.SongId, artist.ArtistId))
            {
                Console.WriteLine("\nSong deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete song. Make sure you own this song.");
            }
        }
        else
        {
            Console.WriteLine("\nDeletion cancelled.");
        }
        
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }


    static void ShowListenerMenu(User user)
    {
        bool running = true;
        string[] options = { 
            "Browse Songs",
            "My Playlists",
            "Create Playlist",
            "Delete Playlist",
            "Logout"
        };

        SongService songService = new();
        PlaylistService playlistService = new();

        while(running)
        {
            int choice = ShowMenu("Listener Menu", options, user.UserName);
            switch(choice)
            {
                case 1:
                    BrowseSongs(songService, playlistService, user);
                    break;
                case 2:
                    ViewMyPlaylists(playlistService, songService, user);
                    break;
                case 3:
                    CreatePlaylist(playlistService, user);
                    break;
                case 4:
                    DeletePlaylistById(playlistService, user);
                    break;
                case 5:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void BrowseSongs(SongService songService, PlaylistService playlistService, User user)
    {
        while (true)
        {
            Console.Clear();
            var songs = songService.GetAllSongs();
            if (!songs.Any())
            {
                Console.WriteLine("No songs available!");
                Console.ReadKey();
                return;
            }

            int currentSelection = 0;
            int currentPage = 0;
            int itemsPerPage = 10;
            int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════╗");
                Console.WriteLine("║               Songs List                 ║");
                Console.WriteLine("╚══════════════════════════════════════════╝");
                
                string header = "╔══════════╦══════════════════════╦══════════════════════╦════════════╗";
                string footer = "╚══════════╩══════════════════════╩══════════════════════╩════════════╝";

                Console.WriteLine(header);
                Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", "Song ID", "Title", "Album", "Genre");
                Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬════════════╣");

                // Display current page items
                var pageItems = songs.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                for (int i = 0; i < pageItems.Count; i++)
                {
                    var song = songs[i];

                    if (i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine("║ {0,-9}║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                                songs[i].SongId.ToString().PadRight(9),
                                Truncate(songs[i].Title, 20),
                                Truncate(songs[i].Album, 20),
                                Truncate(songs[i].Genre, 10));

                    Console.ResetColor();
                }

                Console.WriteLine(footer);
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages}");
                Console.WriteLine("\nUse ↑↓ to navigate songs, ←→ to change pages, Enter to select, Esc to go back");

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0) currentSelection--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentSelection < pageItems.Count - 1) currentSelection++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            currentSelection = 0;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            currentSelection = 0;
                        }
                        break;
                }
            } while (key != ConsoleKey.Enter && key != ConsoleKey.Escape);

            if (key == ConsoleKey.Escape) return;

            // After selecting a song, show options
            var selectedSong = songs[currentSelection];
            string[] options = { "Add to Playlist", "Back" };
            int optionSelection = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\nSelected Song: {selectedSong.Title} by {selectedSong.ArtistName}\n");
                Console.WriteLine("Please choose an option:\n");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == optionSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    
                    Console.WriteLine($"{(i == optionSelection ? "► " : "  ")}{options[i]}");
                    
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (optionSelection > 0) optionSelection--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (optionSelection < options.Length - 1) optionSelection++;
                        break;
                    case ConsoleKey.Enter:
                        switch (optionSelection)
                        {
                            case 0: // Add to Playlist
                                var playlists = playlistService.GetUserPlaylists(user.UserId);
                                DisplayPlaylists(playlists);
                                Console.Write("\nEnter Playlist ID: ");
                                if (int.TryParse(Console.ReadLine(), out int playlistId))
                                {
                                    playlistService.AddSongToPlaylist(playlistId, selectedSong.SongId);
                                    Console.WriteLine("\nSong added to playlist!");
                                    Console.ReadKey();
                                }
                                break;
                            case 1: // Back
                                return;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }

    static void ViewMyPlaylists(PlaylistService playlistService, SongService songService, User user)
    {
        while (true)
        {
            Console.Clear();
            var playlists = playlistService.GetUserPlaylists(user.UserId);
            if (!playlists.Any())
            {
                Console.WriteLine("You don't have any playlists yet!");
                Console.ReadKey();
                return;
            }

            int currentSelection = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════╗");
                Console.WriteLine("║               Your Playlists             ║");
                Console.WriteLine("╚══════════════════════════════════════════╝");
                
                string header = "╔══════════╦══════════════════════╦══════════════════════╗";
                string footer = "╚══════════╩══════════════════════╩══════════════════════╝";

                Console.WriteLine(header);
                Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║", "ID", "Name", "Created Date");
                Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╣");
                
                for (int i = 0; i < playlists.Count; i++)
                {
                    var playlist = playlists[i];
                    if (i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine("║ {0,-8}║ {1,-20} ║ {2,-20} ║",
                                playlists[i].PlaylistId.ToString().PadRight(9),
                                Truncate(playlists[i].PlaylistName, 20),
                                Truncate(playlists[i].CreatedAt.ToString("yyyy-MM-dd"), 20));

                    Console.ResetColor();
                }

                Console.WriteLine(footer);
                Console.WriteLine("\nUse ↑↓ to navigate, Enter to select, Esc to go back");

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0) currentSelection--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentSelection < playlists.Count - 1) currentSelection++;
                        break;
                }
            } while (key != ConsoleKey.Enter && key != ConsoleKey.Escape);

            if (key == ConsoleKey.Escape) return;

            // After selecting a playlist, show songs and options
            var selectedPlaylist = playlists[currentSelection];
            var songs = songService.GetPlaylistSongs(selectedPlaylist.PlaylistId);
            string[] options = { "Remove Song", "Back" };
            int optionSelection = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\nSelected Playlist: {selectedPlaylist.PlaylistName}\n");
                PrintSongTable(songs);
                Console.WriteLine("\nPlease choose an option:");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == optionSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    
                    Console.WriteLine($"{(i == optionSelection ? "► " : "  ")}{options[i]}");
                    
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (optionSelection > 0) optionSelection--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (optionSelection < options.Length - 1) optionSelection++;
                        break;
                    case ConsoleKey.Enter:
                        switch (optionSelection)
                        {
                            case 0: // Remove Song
                                Console.Write("\nEnter Song ID to remove: ");
                                if (int.TryParse(Console.ReadLine(), out int songId))
                                {
                                    playlistService.RemoveSongFromPlaylist(selectedPlaylist.PlaylistId, songId);
                                    Console.WriteLine("Song removed from playlist!");
                                    Console.ReadKey();
                                    songs = songService.GetPlaylistSongs(selectedPlaylist.PlaylistId);
                                }
                                break;
                            case 1: // Back
                                return;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }

    static void CreatePlaylist(PlaylistService playlistService, User user)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║             Create New Playlist          ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        string? name;
        do
        {
            System.Console.Write("Enter Playlist name: ");
            name = Console.ReadLine()?.Trim();
        } while (string.IsNullOrWhiteSpace(name));

        playlistService.CreatePlaylist(name, user.UserId);
        System.Console.WriteLine("Playlist created successfully!");
        ConsoleKey key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.Escape || key == ConsoleKey.Enter)
        {
            return;
        }
    }

    static void DeletePlaylistById(PlaylistService playlistService, User user)
    {
        Console.Clear();
        var playlists = playlistService.GetUserPlaylists(user.UserId);

        if (playlists.Count == 0)
        {  
            Console.WriteLine("You do not any playlists.");
            Console.ReadKey();
            return;
        }

        DisplayPlaylists(playlists);
    
        Console.Write("\nEnter Playlist ID to delete: ");
        string input = Console.ReadLine();

        if (input.ToLower() == "exit" || input == null){
            return;
        }
    
        if (int.TryParse(input, out int playlistId))
        {
            var songs = songService.GetPlaylistSongs(playlistId); // Lấy bài hát bằng ID
        
            Console.Clear();
            PrintSongTable(songs);

            Console.Write("❓ Are you sure you want to delete this song? (Y/N): ");
            string confirm = Console.ReadLine()?.Trim().ToLower();

            if (confirm != "y")
            {
                Console.WriteLine("🚫 Delete canceled.");
                Console.ReadKey();
                return;
            }
        }
        else
        {
            Console.WriteLine("\nInvalid Playlist ID. Please enter a number.");
        }

        Console.ReadKey();
    }


    static void DisplayPlaylists(List<Playlist> playlists)
    {
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║              Your Playlists              ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
    
        string header = "╔══════════╦══════════════════════╦══════════════════════╗";
        string footer = "╚══════════╩══════════════════════╩══════════════════════╝";

                Console.WriteLine(header);
                Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║", "ID", "Name", "Created Date");
                Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╣");
        
        foreach (var playlist in playlists)
        {
            int id = playlist.PlaylistId;
            string name = playlist.PlaylistName;
            string date = playlist.CreatedAt.ToString("yyyy-MM-dd");
            Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║", id, name, date);
        }        
        Console.WriteLine(footer);
    }

    static void PrintSongTable(List<Song> songs)
    {
        string header = "╔═════════╦══════════════════════╦══════════════════════╦═══════════╗";
        string footer = "╚═════════╩══════════════════════╩══════════════════════╩═══════════╝";

        Console.WriteLine(header);
        Console.WriteLine("║ {0,-7} ║ {1,-20} ║ {2,-20} ║ {3,-9} ║",
        "Song ID", "Title", "Album", "Genre");
        Console.WriteLine("╠═════════╬══════════════════════╬══════════════════════╬═══════════╣");

        foreach (var song in songs)
        {
            Console.WriteLine("║ {0,-7} ║ {1,-20} ║ {2,-20} ║ {3,-9} ║",
            song.SongId,
            Truncate(song.Title, 20),
            Truncate(song.Album, 20),
            Truncate(song.Genre, 10));
        }

        Console.WriteLine(footer);
    }


    static string Truncate(string value, int maxLength)
    {
        return string.IsNullOrEmpty(value) 
            ? string.Empty 
            : value.Length <= maxLength 
            ? value 
            : value.Substring(0, maxLength - 3) + "...";
    }
}