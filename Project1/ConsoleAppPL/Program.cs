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
                        case "admin": ShowAdminMenu(user); break;
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
            Console.Write("Username: ");
            string userName = "";
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
            Console.WriteLine();

            // Email input
            Console.Write("Email: ");
            string email = "";
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
        Console.WriteLine("=== My Songs ===\n");
        
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
            Console.WriteLine("\n=== My Songs ===\n");

            // Calculate column widths (removed idWidth)
            int titleWidth = Math.Max("Title".Length, Math.Max(20, songs.Max(s => s.Title?.Length ?? 0)));
            int artistWidth = Math.Max("Artist".Length, Math.Max(20, songs.Max(s => s.ArtistName?.Length ?? 0)));
            int albumWidth = Math.Max("Album".Length, Math.Max(30, songs.Max(s => s.Album?.Length ?? 0)));
            int genreWidth = Math.Max("Genre".Length, Math.Max(15, songs.Max(s => s.Genre?.Length ?? 0)));
            int dateWidth = 10;

            // Create table format strings without ID column
            string horizontalLine = $"╔═{new string('═', titleWidth)}═╦═{new string('═', artistWidth)}═╦═{new string('═', albumWidth)}═╦═{new string('═', genreWidth)}═╦═{new string('═', dateWidth)}═╗";
            string headerLine = $"║ {"Title".PadRight(titleWidth)} ║ {"Artist".PadRight(artistWidth)} ║ {"Album".PadRight(albumWidth)} ║ {"Genre".PadRight(genreWidth)} ║ {"Release".PadRight(dateWidth)} ║";
            string separatorLine = $"╠═{new string('═', titleWidth)}═╬═{new string('═', artistWidth)}═╬═{new string('═', albumWidth)}═╬═{new string('═', genreWidth)}═╬═{new string('═', dateWidth)}═╣";
            string footerLine = $"╚═{new string('═', titleWidth)}═╩═{new string('═', artistWidth)}═╩═{new string('═', albumWidth)}═╩═{new string('═', genreWidth)}═╩═{new string('═', dateWidth)}═╝";

            Console.WriteLine(horizontalLine);
            Console.WriteLine(headerLine);
            Console.WriteLine(separatorLine);

            for (int i = 0; i < songs.Count; i++)
            {
                if (i == currentSelection)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string formattedDate = songs[i].ReleaseDate?.ToString("yyyy-MM-dd") ?? "N/A";
                Console.WriteLine($"║ {songs[i].Title?.PadRight(titleWidth)} ║ " +
                            $"{songs[i].ArtistName?.PadRight(artistWidth)} ║ " +
                            $"{songs[i].Album?.PadRight(albumWidth)} ║ " +
                            $"{songs[i].Genre?.PadRight(genreWidth)} ║ " +
                            $"{formattedDate.PadRight(dateWidth)} ║");

                Console.ResetColor();
            }

            Console.WriteLine(footerLine);
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
        Console.WriteLine("=== Artist Registration ===\n");

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
        Console.WriteLine("=== Upload Song ===\n");

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
        Console.WriteLine("=== Delete Song ===\n");
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
            Console.WriteLine("\n=== Delete Song ===\n");

            // Calculate column widths (without ID)
            int titleWidth = Math.Max("Title".Length, Math.Max(20, songs.Max(s => s.Title?.Length ?? 0)));
            int artistWidth = Math.Max("Artist".Length, Math.Max(20, songs.Max(s => s.ArtistName?.Length ?? 0)));
            int albumWidth = Math.Max("Album".Length, Math.Max(30, songs.Max(s => s.Album?.Length ?? 0)));
            int genreWidth = Math.Max("Genre".Length, Math.Max(15, songs.Max(s => s.Genre?.Length ?? 0)));
            int dateWidth = 10;

            // Create table format strings without ID column
            string horizontalLine = $"╔═{new string('═', titleWidth)}═╦═{new string('═', artistWidth)}═╦═{new string('═', albumWidth)}═╦═{new string('═', genreWidth)}═╦═{new string('═', dateWidth)}═╗";
            string headerLine = $"║ {"Title".PadRight(titleWidth)} ║ {"Artist".PadRight(artistWidth)} ║ {"Album".PadRight(albumWidth)} ║ {"Genre".PadRight(genreWidth)} ║ {"Release".PadRight(dateWidth)} ║";
            string separatorLine = $"╠═{new string('═', titleWidth)}═╬═{new string('═', artistWidth)}═╬═{new string('═', albumWidth)}═╬═{new string('═', genreWidth)}═╬═{new string('═', dateWidth)}═╣";
            string footerLine = $"╚═{new string('═', titleWidth)}═╩═{new string('═', artistWidth)}═╩═{new string('═', albumWidth)}═╩═{new string('═', genreWidth)}═╩═{new string('═', dateWidth)}═╝";

            Console.WriteLine(horizontalLine);
            Console.WriteLine(headerLine);
            Console.WriteLine(separatorLine);

            for (int i = 0; i < songs.Count; i++)
            {
                if (i == currentSelection)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                string formattedDate = songs[i].ReleaseDate?.ToString("yyyy-MM-dd") ?? "N/A";
                Console.WriteLine($"║ {songs[i].Title?.PadRight(titleWidth)} ║ " +
                            $"{songs[i].ArtistName?.PadRight(artistWidth)} ║ " +
                            $"{songs[i].Album?.PadRight(albumWidth)} ║ " +
                            $"{songs[i].Genre?.PadRight(genreWidth)} ║ " +
                            $"{formattedDate.PadRight(dateWidth)} ║");

                Console.ResetColor();
            }

            Console.WriteLine(footerLine);
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

            // Calculate column widths once - removed idWidth
            int titleWidth = Math.Max("Title".Length, Math.Max(20, songs.Max(s => s.Title?.Length ?? 0)));
            int artistWidth = Math.Max("Artist".Length, Math.Max(20, songs.Max(s => s.ArtistName?.Length ?? 0)));
            int albumWidth = Math.Max("Album".Length, Math.Max(30, songs.Max(s => s.Album?.Length ?? 0)));
            int genreWidth = Math.Max("Genre".Length, Math.Max(15, songs.Max(s => s.Genre?.Length ?? 0)));
            int dateWidth = 10;

            do
            {
                Console.Clear();
                Console.WriteLine("\n=== Songs List ===\n");
                
                // Table headers - removed ID column
                string horizontalLine = $"╔═{new string('═', titleWidth)}═╦═{new string('═', artistWidth)}═╦═{new string('═', albumWidth)}═╦═{new string('═', genreWidth)}═╦═{new string('═', dateWidth)}═╗";
                string headerLine = $"║ {"Title".PadRight(titleWidth)} ║ {"Artist".PadRight(artistWidth)} ║ {"Album".PadRight(albumWidth)} ║ {"Genre".PadRight(genreWidth)} ║ {"Release".PadRight(dateWidth)} ║";
                string separatorLine = $"╠═{new string('═', titleWidth)}═╬═{new string('═', artistWidth)}═╬═{new string('═', albumWidth)}═╬═{new string('═', genreWidth)}═╬═{new string('═', dateWidth)}═╣";
                string footerLine = $"╚═{new string('═', titleWidth)}═╩═{new string('═', artistWidth)}═╩═{new string('═', albumWidth)}═╩═{new string('═', genreWidth)}═╩═{new string('═', dateWidth)}═╝";

                Console.WriteLine(horizontalLine);
                Console.WriteLine(headerLine);
                Console.WriteLine(separatorLine);

                // Display current page items
                var pageItems = songs.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                for (int i = 0; i < pageItems.Count; i++)
                {
                    if (i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    string formattedDate = pageItems[i].ReleaseDate?.ToString("yyyy-MM-dd") ?? "N/A";
                    Console.WriteLine($"║ {pageItems[i].Title?.PadRight(titleWidth)} ║ " +
                                    $"{pageItems[i].ArtistName?.PadRight(artistWidth)} ║ " +
                                    $"{pageItems[i].Album?.PadRight(albumWidth)} ║ " +
                                    $"{pageItems[i].Genre?.PadRight(genreWidth)} ║ " +
                                    $"{formattedDate.PadRight(dateWidth)} ║");

                    Console.ResetColor();
                }

                Console.WriteLine(footerLine);
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
            string[] options = { "Add to Playlist", "Play Song", "Back" };
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
                            case 1: // Play Song
                                Console.WriteLine($"\nPlaying {selectedSong.Title}...");
                                Console.ReadKey();
                                break;
                            case 2: // Back
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
                Console.WriteLine("\n=== Your Playlists ===\n");

                // Calculate widths
                int idWidth = Math.Max("ID".Length, playlists.Max(p => p.PlaylistId.ToString().Length));
                int nameWidth = Math.Max("Name".Length, playlists.Max(p => p.PlaylistName.Length));
                int dateWidth = Math.Max("Created Date".Length, 10);

                // Add padding
                idWidth += 8;
                nameWidth += 4;
                dateWidth += 4;

                // Create table format
                string header = $"╔═{new string('═', idWidth)}═╦═{new string('═', nameWidth)}═╦═{new string('═', dateWidth)}═╗";
                string titleRow = $"║ {"ID".PadRight(idWidth)} ║ {"Name".PadRight(nameWidth)} ║ {"Created Date".PadRight(dateWidth)} ║";
                string separator = $"╠═{new string('═', idWidth)}═╬═{new string('═', nameWidth)}═╬═{new string('═', dateWidth)}═╣";
                string footer = $"╚═{new string('═', idWidth)}═╩═{new string('═', nameWidth)}═╩═{new string('═', dateWidth)}═╝";

                Console.WriteLine(header);
                Console.WriteLine(titleRow);
                Console.WriteLine(separator);

                for (int i = 0; i < playlists.Count; i++)
                {
                    if (i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine($"║ {playlists[i].PlaylistId.ToString().PadRight(idWidth)} ║ " +
                                $"{playlists[i].PlaylistName.PadRight(nameWidth)} ║ " +
                                $"{playlists[i].CreatedAt.ToString("yyyy-MM-dd").PadRight(dateWidth)} ║");

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
            string[] options = { "Remove Song", "Play Song", "Back" };
            int optionSelection = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\nSelected Playlist: {selectedPlaylist.PlaylistName}\n");
                DisplaySongs(songs);
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
                            case 1: // Play Song
                                Console.Write("\nEnter Song ID to play: ");
                                if (int.TryParse(Console.ReadLine(), out int playSongId))
                                {
                                    Console.WriteLine($"\nPlaying song... (Feature coming soon)");
                                    Console.ReadKey();
                                }
                                break;
                            case 2: // Back
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
        Console.WriteLine("=== Create New Playlist ===\n");
        
        string? name;
        do
        {
            Console.Write("Enter playlist name: ");
            name = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(name));

        playlistService.CreatePlaylist(name, user.UserId);
        Console.WriteLine("\nPlaylist created successfully!");
        Console.ReadKey();
    }

    static void DisplayPlaylists(List<Playlist> playlists)
    {
        // Calculate max widths without ID column
        int nameWidth = Math.Max("Name".Length, 
                                playlists.Max(p => p.PlaylistName.Length));
        int dateWidth = Math.Max("Created Date".Length, 10); // "yyyy-MM-dd"

        // Add padding to make columns look better
        nameWidth += 4;   // Add extra space for Name column
        dateWidth += 4;   // Add extra space for Date column

        // Helper function to center text
        string CenterText(string text, int width)
        {
            if (string.IsNullOrEmpty(text)) text = "";
            text = text.Length > width ? text.Substring(0, width) : text;
            int spaces = width - text.Length;
            int leftPad = spaces / 2;
            return text.PadLeft(leftPad + text.Length).PadRight(width);
        }

        // Create borders with exact spacing - removed ID column
        string header =    $"╔═{new string('═', nameWidth)}═╦═{new string('═', dateWidth)}═╗";
        string titleRow =  $"║ {CenterText("Name", nameWidth)} ║ {CenterText("Created Date", dateWidth)} ║";
        string separator = $"╠═{new string('═', nameWidth)}═╬═{new string('═', dateWidth)}═╣";
        string footer =    $"╚═{new string('═', nameWidth)}═╩═{new string('═', dateWidth)}═╝";

        Console.WriteLine("\n=== Your Playlists ===");
        Console.WriteLine(header);
        Console.WriteLine(titleRow);
        Console.WriteLine(separator);
        
        foreach (var playlist in playlists)
        {
            string name = playlist.PlaylistName;
            string date = playlist.CreatedAt.ToString("yyyy-MM-dd");
            
            Console.WriteLine($"║ {CenterText(name, nameWidth)} ║ {CenterText(date, dateWidth)} ║");
        }
        
        Console.WriteLine(footer);
    }

    static void DisplaySongs(List<Song> songs)
    {
        Console.WriteLine("\n=== Songs ===");
        
        // Calculate column widths - remove idWidth
        int titleWidth = Math.Max("Title".Length, 
                            Math.Max(20, songs.Max(s => s.Title?.Length ?? 0)));
        int artistWidth = Math.Max("Artist".Length,
                            Math.Max(20, songs.Max(s => s.ArtistName?.Length ?? 0)));
        int albumWidth = Math.Max("Album".Length,
                            Math.Max(30, songs.Max(s => s.Album?.Length ?? 0)));
        int genreWidth = Math.Max("Genre".Length,
                            Math.Max(15, songs.Max(s => s.Genre?.Length ?? 0)));
        int dateWidth = 10;  // Fixed for yyyy-MM-dd format

        // Create table format strings without ID column
        string horizontalLine = $"╔═{new string('═', titleWidth)}═╦═{new string('═', artistWidth)}═╦═{new string('═', albumWidth)}═╦═{new string('═', genreWidth)}═╦═{new string('═', dateWidth)}═╗";
        string headerLine =    $"║ {"Title".PadRight(titleWidth)} ║ {"Artist".PadRight(artistWidth)} ║ {"Album".PadRight(albumWidth)} ║ {"Genre".PadRight(genreWidth)} ║ {"Release".PadRight(dateWidth)} ║";
        string separatorLine = $"╠═{new string('═', titleWidth)}═╬═{new string('═', artistWidth)}═╬═{new string('═', albumWidth)}═╬═{new string('═', genreWidth)}═╬═{new string('═', dateWidth)}═╣";
        string footerLine =    $"╚═{new string('═', titleWidth)}═╩═{new string('═', artistWidth)}═╩═{new string('═', albumWidth)}═╩═{new string('═', genreWidth)}═╩═{new string('═', dateWidth)}═╝";

        Console.WriteLine(horizontalLine);
        Console.WriteLine(headerLine);
        Console.WriteLine(separatorLine);

        foreach (var song in songs)
        {
            string formattedDate = song.ReleaseDate?.ToString("yyyy-MM-dd") ?? "N/A";
            Console.WriteLine($"║ {song.Title?.PadRight(titleWidth)} ║ " +
                        $"{song.ArtistName?.PadRight(artistWidth)} ║ " +
                        $"{song.Album?.PadRight(albumWidth)} ║ " +
                        $"{song.Genre?.PadRight(genreWidth)} ║ " +
                        $"{formattedDate.PadRight(dateWidth)} ║");
        }

        Console.WriteLine(footerLine);
    }

}