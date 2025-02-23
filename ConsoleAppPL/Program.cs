using System.Text;
using BL;
using BL.Services;
using Persistence.Models;
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

    private static ArtistService artistService = new ArtistService();
    private static SongService songService = new SongService();
    private static PlaylistService playlistService = new PlaylistService();

    static void Main(string[] args)
    {
        var userService = new UserService();
        var artistService = new ArtistService();
        var songService = new SongService();
        var playlistService = new PlaylistService();
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
        ShowManagePlaylistsMenu(playlistService);
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
                    case "admin": 
                        ShowAdminMenu(user, userService); // 🔹 FIXED: Thêm 'userService'
                        break;

                    case "artist": 
                        ShowArtistMenu(user); 
                        break;

                    case "listener": 
                        ShowListenerMenu(user); 
                        break;

                    default:
                        Console.WriteLine("❌ Unknown role. Access denied!");
                        Console.ReadKey();
                        break;
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

    static void ShowAdminMenu(User user, UserService userService)
    {
        bool running = true;
        string[] options = { "Manage Users", "Manage Artists", "Manage Songs", "Manage Playlists", "Logout" };

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

                case 4:
                ShowManagePlaylistsMenu(playlistService);
                break;

                case 5: running = false; break;
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

        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║             📜 All Users                 ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        if (users.Count == 0)
        {
            Console.WriteLine("❌ No users found.");
            return;
        }

        // Định dạng bảng
        string header = "╔══════════╦══════════════════════╦══════════════════════╦══════════╗";
        string footer = "╚══════════╩══════════════════════╩══════════════════════╩══════════╝";
    
        Console.WriteLine(header);
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-8} ║", 
                    "User ID", "User Name", "Email", "Role");
        Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬══════════╣");

        foreach (var user in users)
        {
            Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-8} ║",
                        user.UserId,
                        Truncate(user.UserName, 20),
                        Truncate(user.UserEmail, 20),
                        Truncate(user.Roles, 8));
        }
    
        Console.WriteLine(footer);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
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

        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║             📜 All Artists               ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        if (artists == null || artists.Count == 0) // Kiểm tra danh sách rỗng
        {
            Console.WriteLine("❌ No artist found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        // Định dạng bảng
        string header = "╔═══════════╦══════════════════════╦══════════════════════╦════════════╗";
        string footer = "╚═══════════╩══════════════════════╩══════════════════════╩════════════╝";

        Console.WriteLine(header);
        Console.WriteLine("║ {0,-9} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", 
        "Artist ID", "Artist Name", "Top Song", "Birth Date");
        Console.WriteLine("╠═══════════╬══════════════════════╬══════════════════════╬════════════╣");

        // Duyệt qua danh sách nghệ sĩ và in từng dòng
        foreach (var artist in artists)
        {
            Console.WriteLine("║ {0,-9} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                artist.ArtistId,
                Truncate(artist.ArtistName, 20),
                Truncate(artist.TopSong, 20),
                artist.BirthDate?.ToString("yyyy-MM-dd") ?? "N/A");
        }

        Console.WriteLine(footer);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
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

        // Thực hiện xóa nghệ sĩ
        bool result = songService.DeleteSong(songId);
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

        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║             📜 All Songs                 ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        if (songs == null || songs.Count == 0) // Kiểm tra danh sách rỗng
        {
            Console.WriteLine("❌ No song found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        // Định dạng bảng
        string header = "╔══════════╦══════════════════════╦══════════════════════╦════════════╗";
        string footer = "╚══════════╩══════════════════════╩══════════════════════╩════════════╝";

        Console.WriteLine(header);
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║ {3,-10} ║", "Song ID", "Title", "Album", "Genre");
        Console.WriteLine("╠══════════╬══════════════════════╬══════════════════════╬════════════╣");

        foreach (var song in songs)
        {
            Console.WriteLine("║ {0,-9}║ {1,-20} ║ {2,-20} ║ {3,-10} ║",
                song.SongId,
            Truncate(song.Title, 20),
            Truncate(song.Album, 20),
            Truncate(song.Genre, 10));
        }
        Console.WriteLine(footer);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    
    static void ShowManagePlaylistsMenu(PlaylistService playlistService)
    {
        bool running = true;
        string[] userOptions = { "Search Playlist", "Delete Playlist", "Show All Playlists", "Back to Admin Menu" };

        while (running)
        {
            int choice = ShowMenu("Manage Playlists", userOptions, "Admin");
            switch (choice)
            {
                case 1:
                    GetPlaylistById(playlistService);
                    break;
                    
                
                case 2:
                    DeletePlaylistById(playlistService);
                    break;

                case 3:
                    ShowAllPlaylists(playlistService);
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

    static void GetPlaylistById(PlaylistService playlistService)
    {
        Console.Clear();
        Console.Write("🔍 Enter Playlist ID: ");

        if (!int.TryParse(Console.ReadLine(), out int playlistId))  // Sửa tên biến
        {
            Console.WriteLine("❌ Invalid ID! Please enter a number.");
            Console.ReadKey();
            return;
        }

        var playlist = playlistService?.SearchPlaylist(playlistId);  // Kiểm tra null
        if (playlist == null)
        {
            Console.WriteLine("❌ No playlist found.");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║              🔎 Playlist Found           ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        string header = "╔═════════════╦══════════════════════╦══════════════════════╗";
        string footer = "╚═════════════╩══════════════════════╩══════════════════════╝";

        Console.WriteLine(header);
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║", "Playlist ID", "Playlist", "Title");
        Console.WriteLine("╠═════════════╬══════════════════════╬══════════════════════╣");
        
        foreach (var song in playlist.Songs)
        {
            Console.WriteLine("║ {0,-8}    ║ {1,-20} ║ {2,-20} ║",
            playlist.PlaylistId,
            Truncate(playlist.PlaylistName, 20),
            Truncate(song.Title, 20));
        }
        Console.WriteLine(footer);

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void DeletePlaylistById(PlaylistService playlistService)
    {
        Console.Clear();
        Console.Write("🗑 Enter Playlist ID to delete: ");
    
        // Bước 1: Nhập và validate Playlist ID
        if (!int.TryParse(Console.ReadLine(), out int playlistId))
        {
            Console.WriteLine("❌ Invalid ID! Please enter a number.");
            Console.ReadKey();
            return;
        }

        // Bước 2: Tìm playlist để hiển thị thông tin
        var playlist = playlistService.SearchPlaylist(playlistId);
        if (playlist == null)
        {
            Console.WriteLine("❌ No playlist found with that ID.");
            Console.ReadKey();
            return;
        }

        // Bước 3: Hiển thị thông tin playlist
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║              🔎 Playlist Found           ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        string header = "╔═════════════╦══════════════════════╦══════════════════════╗";
        string footer = "╚═════════════╩══════════════════════╩══════════════════════╝";

        Console.WriteLine(header);
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║", "Playlist ID", "Playlist", "Title");
        Console.WriteLine("╠═════════════╬══════════════════════╬══════════════════════╣");
        
        foreach (var song in playlist.Songs)
        {
            Console.WriteLine("║ {0,-8}    ║ {1,-20} ║ {2,-20} ║",
            playlist.PlaylistId,
            Truncate(playlist.PlaylistName, 20),
            Truncate(song.Title, 20));
        }
        Console.WriteLine(footer);

        // Bước 4: Xác nhận xóa
        Console.Write("\n❓ Are you sure you want to delete this playlist? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToLower();

        if (confirm != "y")
        {
            Console.WriteLine("🚫 Delete canceled.");
            Console.ReadKey();
            return;
        }

        // Bước 5: Thực hiện xóa
        bool result = playlistService.DeletePlaylist(playlistId);
        if (result)
        {
            Console.WriteLine("\n✅ Playlist deleted successfully.");
        }
        else
        {
            Console.WriteLine("\n❌ Failed to delete playlist.");
        }
        Console.ReadKey();
    }

    static void ShowAllPlaylists(PlaylistService playlistService)
    {
        Console.Clear();
        var playlists = playlistService.GetAllPlaylists();

        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║             📜 All Playlists             ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");

        if (playlists == null || playlists.Count == 0) // Kiểm tra danh sách rỗng
        {
            Console.WriteLine("❌ No playlist found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        // Định dạng bảng
        string header = "╔═════════════╦══════════════════════╦══════════════════════╗";
        string footer = "╚═════════════╩══════════════════════╩══════════════════════╝";

        Console.WriteLine(header);
        Console.WriteLine("║ {0,-8} ║ {1,-20} ║ {2,-20} ║", "Playlist ID", "Playlist", "Title");
        Console.WriteLine("╠═════════════╬══════════════════════╬══════════════════════╣");
        
        foreach (var playlist in playlists)  // Duyệt qua từng playlist
        {
            foreach (var song in playlist.Songs)  // Duyệt qua từng bài hát trong playlist
            {
                Console.WriteLine("║ {0,-8}    ║ {1,-20} ║ {2,-20} ║",
                    playlist.PlaylistId,
                    Truncate(playlist.PlaylistName, 20),
                    Truncate(song.Title, 20));
            }
        }

        Console.WriteLine(footer);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }


    static void ShowArtistMenu(User user)
    {
        bool running = true;
        string[] options = { 
            "Register basic information",
            "Upload Song", 
            "Upload Playlist",
            "View My Songs",
            "Logout"
        };

        ArtistService artistService = new ArtistService();
        SongService songService = new SongService();
        PlaylistService playlistService = new PlaylistService();

        while(running)
        {
            int choice = ShowMenu("Artist Menu", options, user.UserName);
            switch(choice)
            {
                case 1:
                    RegisterArtist(artistService);
                    break;
                case 2:
                    UploadSong(songService);
                    break;
                case 3:
                    UploadPlaylist(playlistService);
                    break;
                case 4:
                    ViewSongs();
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

    static void RegisterArtist(ArtistService artistService)
    {
        Console.Clear();
        Console.WriteLine("Registering a new artist...");

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
        Console.WriteLine("Invalid date format. Please enter again.");
        }

        string? topSong;
        do
        {
            Console.Write("Enter top song: ");
            topSong = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(topSong));

        artistService.RegisterArtist(name, birthDate, topSong);
        Console.WriteLine("\nArtist registered successfully!");
        Console.ReadKey();
    }

    static void UploadSong(SongService songService)
    {
        Console.Clear();

        string? title;
        do
        {
            Console.Write("Enter title of song: ");
            title = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(title));

        int artistId;
        while (true)
        {
            Console.Write("Enter artistId of song: ");
            if (int.TryParse(Console.ReadLine(), out artistId))
            {
                break;
            }
            Console.WriteLine("Invalid input. Enter artistId again.");
        }

        string? album;
        do
        {
            Console.Write("Enter album of song: ");
            album = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(album));

        string? genre;
        do
        {
            Console.Write("Enter genre of song: ");
            genre = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(genre));

        DateTime releaseDate;
        while (true)
        {
            Console.Write("Enter release date (YYYY-MM-DD): ");
            string? input = Console.ReadLine();
            if (DateTime.TryParse(input, out releaseDate))
            {
                break;
            }
            Console.WriteLine("Invalid date format. Please enter again.");
        }

        songService.UploadSong(title, artistId, album, genre, releaseDate);
        Console.WriteLine("\nSong uploaded successfully!");
        Console.ReadKey();
    }

    static void UploadPlaylist(PlaylistService playlistService)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════╗");
        Console.WriteLine("║         Upload Playlist          ║");
        Console.WriteLine("╚══════════════════════════════════╝");

        // Nhập tên playlist
        string? playlistName;
        do
        {
            Console.Write("Enter playlist name: ");
            playlistName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(playlistName));

        // Danh sách các bài hát trong playlist
        List<Song> songs = new List<Song>();

        // Nhập các bài hát
        bool addingSongs = true;
        while (addingSongs)
        {
            Console.WriteLine("\nAdd a new song to the playlist:");

            // Nhập thông tin bài hát
            string? title;
            do
            {
                Console.Write("Enter title of song: ");
                title = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(title));

            int artistId;
            while (true)
            {
                Console.Write("Enter artistId of song: ");
                if (int.TryParse(Console.ReadLine(), out artistId))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Enter artistId again.");
            }

            string? album;
            do
            {
                Console.Write("Enter album of song: ");
                album = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(album));

            string? genre;
            do
            {
                Console.Write("Enter genre of song: ");
                genre = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(genre));

            DateTime releaseDate;
            while (true)
            {
                Console.Write("Enter release date (YYYY-MM-DD): ");
                string? input = Console.ReadLine();
                if (DateTime.TryParse(input, out releaseDate))
                {
                    break;
                }
                Console.WriteLine("Invalid date format. Please enter again.");
            }

            // Tạo đối tượng Song và thêm vào danh sách
            Song song = new Song(title, artistId, album, genre, releaseDate);
            songs.Add(song);

            // Hỏi người dùng có muốn thêm bài hát khác không
            Console.Write("Do you want to add another song? (y/n): ");
            string? addMore = Console.ReadLine();
            if (addMore?.ToLower() != "y")
            {
                addingSongs = false;
            }
        }

        // Gọi service để tạo playlist và thêm bài hát
        bool result = playlistService.CreatePlaylist(playlistName, songs);

        if (result)
        {
            Console.WriteLine("\nPlaylist uploaded successfully!");
        }
        else
        {
            Console.WriteLine("\nFailed to upload playlist.");
        }
        Console.ReadKey();
    }

    
    static void ViewSongs()
    {
        SongService songService = new SongService();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║      View Artist's Songs         ║");
            Console.WriteLine("╚══════════════════════════════════╝");

            // Nhập Artist ID
            int artistId;
            while (true)
            {
                Console.Write("Enter Artist ID (0 to exit): ");
                string input = Console.ReadLine()?.Trim();

                if (input == "0") return; // Thoát nếu nhập 0

                if (int.TryParse(input, out artistId) && artistId > 0)
                {
                    break;
                }

                Console.WriteLine("Invalid input! Please enter a valid Artist ID.");
            }

            // Lấy danh sách bài hát
            try
            {
                List<Song> songs = songService.GetSongByArtistId(artistId);

                // Hiển thị kết quả
                if (songs == null || songs.Count == 0)
                {
                    Console.WriteLine("\nNo songs found for this artist.");
                }
                else
                {
                    Console.WriteLine("╔══════════════════════════════════╗");
                    Console.WriteLine("║              Song List           ║");
                    Console.WriteLine("╚══════════════════════════════════╝");
                    PrintSongTable(songs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError fetching songs: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
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