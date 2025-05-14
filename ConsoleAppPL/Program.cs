using System.Text;
using BL;
using Persistence;
using Persistence.Models;

class Program
{
    private static string GetMaskedInput(bool allowEscape = true)
    {
        var password = new StringBuilder();
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (allowEscape && key.Key == ConsoleKey.Escape)
                return null;

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
            string? password = GetMaskedInput(true);
            if (password == null)
                return;
            
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
            string userName = "";
            bool isValidUsername = false;
            
            while (!isValidUsername)
            {
                // Reset cursor position for username input
                Console.SetCursorPosition(0, 3);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, 3);
                
                Console.Write("Username: ");
                userName = "";
                int cursorLeft = "Username: ".Length;
                Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                
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
                
                // Check if username exists - clear any previous messages
                if (userService.CheckUserNameExists(userName))
                {
                    // Display the error message
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Username already exists! Please try a different one.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey(true);
                    
                    // Clear all lines including the error message
                    Console.SetCursorPosition(0, 3);  // Go back to username line
                    Console.Write(new string(' ', Console.WindowWidth));  // Clear username line
                    Console.SetCursorPosition(0, 4);  // Go to first error message line
                    Console.Write(new string(' ', Console.WindowWidth));  // Clear first error message
                    Console.SetCursorPosition(0, 5);  // Go to second error message line
                    Console.Write(new string(' ', Console.WindowWidth));  // Clear second error message
                    
                    // We don't need to reset the cursor position here since the loop will set it back to line 3
                }
                else
                {
                    isValidUsername = true;
                }
            }

            // Email input 
            string email = "";
            bool isValidEmail = false;
            
            while (!isValidEmail)
            {
                // Reset cursor position for email input
                Console.SetCursorPosition(0, 4);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, 4);
                
                Console.Write("Email: ");
                email = "";
                int cursorLeft = "Email: ".Length;
                Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                
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
                
                // Check email format and existence - clear any previous messages
                if (!userService.IsValidEmail(email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid email format. Please try again.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey(true);
                    Console.SetCursorPosition(0, 5);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, 6);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, 5);
                    Console.Write(new string(' ', Console.WindowWidth));
                    continue;
                }
                
                if (userService.CheckUserEmailExists(email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Email already exists! Please try a different one.");
                    Console.ResetColor();
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey(true);
                    Console.SetCursorPosition(0, 5);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, 6);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, 5);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                else
                {
                    isValidEmail = true;
                }
            }

            // Password input
            Console.SetCursorPosition(0, 5);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 5);
            Console.Write("Password: ");
            string? password = GetMaskedInput(true);
            if (password == null)
                return;

            // Role selection after validating all inputs
            Console.Clear();
            Console.WriteLine("╔════════════════╗");
            Console.WriteLine("║    Sign Up     ║");
            Console.WriteLine("╚════════════════╝");
            Console.WriteLine($"\nUsername: {userName}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Password: {"*".PadRight(password.Length, '*')}");

            // Role selection using ShowMenu
            string[] roleOptions = { "Listener", "Artist" };
            int roleChoice = ShowMenu("Select Role", roleOptions);
            string role = (roleChoice == 1) ? "listener" : "artist";

            // Complete sign up
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                if (userService.SignUp(userName, email, password, role))
                {
                    Console.WriteLine("\nSign up successful!");
                }
                else
                {
                    Console.WriteLine("\nSign up failed! An unexpected error occurred.");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
                return;
            }
        }
    }
    static void ShowAdminMenu(User user)
    {
        Console.Clear();
        bool running = true;
        string[] options = { 
            "Manage Users", 
            "Manage Songs",
            "Manage Playlists",
            "Manage Artists",
            "Logout"
        };

        UserService userService = new UserService();
        SongService songService = new SongService();
        PlaylistService playlistService = new PlaylistService();
        ArtistService artistService = new ArtistService();

        while(running)
        {
            int choice = ShowMenu("Admin Menu", options, user.UserName);
            switch(choice)
            {
                case 1:
                    ManageUsers(userService);
                    break;
                case 2:
                    ManageSongs(songService);
                    break;
                case 3:
                    ManagePlaylists(playlistService);
                    break;
                case 4:
                    ManageArtists(artistService);
                    break;
                case 5: 
                    running = false;
                    break;
            }
        }
    }

    static void ManageUsers(UserService userService)
    {
        bool running = true;
        int currentSelection = 0;
        string searchTerm = "";
        bool isSearching = false;
        int currentPage = 0;
        int itemsPerPage = 10; // Số lượng người dùng mỗi trang
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Users ===\n");
            
            // Lấy danh sách người dùng từ service
            List<User> users;
            if (string.IsNullOrEmpty(searchTerm))
                users = userService.GetAllUsers();
            else
                users = userService.AdminSearchUsers(searchTerm);
                    
            if (users.Count == 0)
            {
                Console.WriteLine(string.IsNullOrEmpty(searchTerm) 
                    ? "No users found!" 
                    : $"No users found matching '{searchTerm}'");
            }
            else
            {
                // Tính toán số trang
                int totalPages = (int)Math.Ceiling(users.Count / (double)itemsPerPage);
                
                // Đảm bảo currentPage hợp lệ
                if (currentPage >= totalPages)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;
                    
                // Lấy danh sách người dùng cho trang hiện tại
                var pageUsers = users
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
                    
                // Hiển thị danh sách users cho trang hiện tại
                DisplayUsers(pageUsers, currentSelection);
                
                // Hiển thị thông tin phân trang
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages} - Showing {pageUsers.Count} of {users.Count} users");
                
                // Hiển thị người dùng được chọn
                if (currentSelection >= pageUsers.Count)
                    currentSelection = pageUsers.Count - 1;
                    
                if (!isSearching && pageUsers.Count > 0 && currentSelection >= 0)
                {
                    Console.WriteLine("\nSelected user:");
                    User selectedUser = pageUsers[currentSelection];
                    Console.WriteLine($"ID: {selectedUser.UserId}, Username: {selectedUser.UserName}, Role: {selectedUser.Roles}");
                }
            }
            
            // Hiển thị thanh tìm kiếm nếu đang tìm kiếm
            if (isSearching)
            {
                Console.Write($"\nSearch: {searchTerm}");
            }
            else
            {
                // Hiển thị hướng dẫn
                Console.WriteLine("\nControls:");
                Console.WriteLine("↑↓: Navigate users • ←→: Change pages • S: Search • D: Delete user • Enter: Options • Esc: Back");
            }
            
            // Xử lý input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
            if (isSearching)
            {
                // Xử lý input khi đang tìm kiếm
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isSearching = false;
                    currentSelection = 0;
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isSearching = false;
                    searchTerm = ""; // Clear search
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTerm.Length > 0)
                {
                    searchTerm = searchTerm[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTerm += keyInfo.KeyChar;
                }
            }
            else
            {
                // Xử lý input khi không tìm kiếm
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                            currentSelection--;
                        break;
                        
                    case ConsoleKey.DownArrow:
                        {
                            var pageUsers = users.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                            if (currentSelection < pageUsers.Count - 1)
                                currentSelection++;
                        }
                        break;
                    
                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        {
                            int totalPages = (int)Math.Ceiling(users.Count / (double)itemsPerPage);
                            if (currentPage < totalPages - 1)
                            {
                                currentPage++;
                                currentSelection = 0; // Reset selection khi chuyển trang
                            }
                        }
                        break;
                        
                    case ConsoleKey.S:
                        isSearching = true;
                        searchTerm = "";
                        currentPage = 0; // Reset về trang đầu khi tìm kiếm
                        break;
                        
                    case ConsoleKey.D:
                        {
                            var pageUsers = users.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                            if (pageUsers.Count > 0 && currentSelection >= 0 && currentSelection < pageUsers.Count)
                            {
                                // Xóa người dùng được chọn
                                User selectedUser = pageUsers[currentSelection];
                                DeleteSelectedUser(selectedUser, userService);
                            }
                        }
                        break;
                        
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                        
                    case ConsoleKey.Enter:
                        {
                            var pageUsers = users.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                            if (pageUsers.Count > 0 && currentSelection >= 0 && currentSelection < pageUsers.Count)
                            {
                                // Hiển thị chi tiết và tùy chọn cho người dùng được chọn
                                User selectedUser = pageUsers[currentSelection];
                                ShowUserOptions(selectedUser, userService);
                            }
                        }
                        break;
                }
            }
        }
    }


    // Hiển thị tùy chọn cho người dùng đã chọn
    static void ShowUserOptions(User user, UserService userService)
    {
        string[] options = { "Delete User", "Back" };
        int choice = ShowMenu($"User: {user.UserName}", options);
        
        switch (choice)
        {              
            case 1:
                // Xóa người dùng
                DeleteSelectedUser(user, userService);
                break;
        }
    }

    // Xóa người dùng đã chọn
    static void DeleteSelectedUser(User user, UserService userService)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete user '{user.UserName}'?");
        Console.WriteLine("This action cannot be undone.");
        Console.WriteLine("\nPress Enter to confirm, any other key to cancel...");
        
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            // Sử dụng chức năng DeleteUser từ UserService
            if (userService.AdminDeleteUser(user.UserId))
            {
                Console.WriteLine("\nUser deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete user!");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    static void ManagePlaylists(PlaylistService playlistService)
    {
        bool running = true;
        int currentSelection = 0;
        string searchTerm = "";
        bool isSearching = false;
        int currentPage = 0;
        int itemsPerPage = 10; // 10 playlist mỗi trang
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Playlists ===\n");
            
            // Lấy danh sách playlist từ service
            List<Playlist> playlists;
            if (string.IsNullOrEmpty(searchTerm))
                playlists = playlistService.GetAllPlaylists();
            else
                playlists = playlistService.AdminSearchPlaylists(searchTerm);
                        
            if (playlists.Count == 0)
            {
                Console.WriteLine(string.IsNullOrEmpty(searchTerm) 
                    ? "No playlists found!" 
                    : $"No playlists found matching '{searchTerm}'");
            }
            else
            {
                // Tính toán số trang
                int totalPages = (int)Math.Ceiling(playlists.Count / (double)itemsPerPage);
                
                // Đảm bảo currentPage hợp lệ
                if (currentPage >= totalPages)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;
                    
                // Lấy danh sách playlist cho trang hiện tại
                var pagePlaylists = playlists
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
                    
                // Hiển thị danh sách playlists cho trang hiện tại
                DisplayPlaylists(pagePlaylists, currentSelection);
                
                // Hiển thị thông tin phân trang
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages} - Showing {pagePlaylists.Count} of {playlists.Count} playlists");
                
                // Hiển thị playlist được chọn
                if (currentSelection >= pagePlaylists.Count)
                    currentSelection = pagePlaylists.Count - 1;
                    
                if (!isSearching && pagePlaylists.Count > 0 && currentSelection >= 0)
                {
                    Console.WriteLine("\nSelected playlist:");
                    Playlist selectedPlaylist = pagePlaylists[currentSelection];
                    Console.WriteLine($"ID: {selectedPlaylist.PlaylistId}, Name: {selectedPlaylist.PlaylistName}, Creator: {selectedPlaylist.Username}, Created At: {selectedPlaylist.CreatedAt.ToString("yyyy-MM-dd")}");
                }
            }
            
            // Hiển thị thanh tìm kiếm nếu đang tìm kiếm
            if (isSearching)
            {
                Console.Write($"\nSearch: {searchTerm}");
            }
            else
            {
                // Hiển thị hướng dẫn
                Console.WriteLine("\nControls:");
                Console.WriteLine("↑↓: Navigate playlists • ←→: Change pages • S: Search • D: Delete playlist • Enter: Options • Esc: Back");
            }
            
            // Xử lý input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
            if (isSearching)
            {
                // Xử lý input khi đang tìm kiếm
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isSearching = false;
                    currentSelection = 0;
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isSearching = false;
                    searchTerm = ""; // Clear search
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTerm.Length > 0)
                {
                    searchTerm = searchTerm[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTerm += keyInfo.KeyChar;
                }
            }
            else
            {
                // Xử lý input khi không tìm kiếm
                var pagePlaylists = playlists.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                            currentSelection--;
                        break;
                        
                    case ConsoleKey.DownArrow:
                        if (currentSelection < pagePlaylists.Count - 1)
                            currentSelection++;
                        break;
                    
                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        int totalPages = (int)Math.Ceiling(playlists.Count / (double)itemsPerPage);
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.S:
                        isSearching = true;
                        searchTerm = "";
                        currentPage = 0; // Reset về trang đầu khi tìm kiếm
                        break;
                        
                    case ConsoleKey.D:
                        if (pagePlaylists.Count > 0 && currentSelection >= 0 && currentSelection < pagePlaylists.Count)
                        {
                            // Xóa playlist được chọn
                            Playlist selectedPlaylist = pagePlaylists[currentSelection];
                            DeleteSelectedPlaylist(selectedPlaylist, playlistService);
                        }
                        break;
                        
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                        
                    case ConsoleKey.Enter:
                        if (pagePlaylists.Count > 0 && currentSelection >= 0 && currentSelection < pagePlaylists.Count)
                        {
                            // Hiển thị chi tiết và tùy chọn cho playlist được chọn
                            Playlist selectedPlaylist = pagePlaylists[currentSelection];
                            ShowPlaylistOptions(selectedPlaylist, playlistService);
                        }
                        break;
                }
            }
        }
    }

    // Hiển thị tùy chọn cho playlist đã chọn
    static void ShowPlaylistOptions(Playlist playlist, PlaylistService playlistService)
    {
        string[] options = { "Delete Playlist", "Back" };
        int choice = ShowMenu($"Playlist: {playlist.PlaylistName}", options);
        
        switch (choice)
        {              
            case 1:
                // Xóa playlist
                DeleteSelectedPlaylist(playlist, playlistService);
                break;
        }
    }

    // Xóa playlist đã chọn
    static void DeleteSelectedPlaylist(Playlist playlist, PlaylistService playlistService)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete playlist '{playlist.PlaylistName}'?");
        Console.WriteLine("This action cannot be undone and will remove all songs from this playlist.");
        Console.WriteLine("\nPress Enter to confirm, any other key to cancel...");
        
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            // Phương thức DeletePlaylist từ PlaylistService cho admin
            if (playlistService.DeletePlaylist(playlist.PlaylistId, 0, "admin")) // Admin không cần kiểm tra quyền
            {
                Console.WriteLine("\nPlaylist deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete playlist!");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    static void ManageArtists(ArtistService artistService)
    {
        bool running = true;
        int currentSelection = 0;
        string searchTerm = "";
        bool isSearching = false;
        int currentPage = 0;
        int itemsPerPage = 10; // 10 nghệ sĩ mỗi trang
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Artists ===\n");
            
            // Lấy danh sách nghệ sĩ từ service
            List<Artist> artists;
            if (string.IsNullOrEmpty(searchTerm))
                artists = artistService.GetAllArtists();
            else
                artists = artistService.AdminSearchArtists(searchTerm);
                        
            if (artists.Count == 0)
            {
                Console.WriteLine(string.IsNullOrEmpty(searchTerm) 
                    ? "No artists found!" 
                    : $"No artists found matching '{searchTerm}'");
            }
            else
            {
                // Tính toán số trang
                int totalPages = (int)Math.Ceiling(artists.Count / (double)itemsPerPage);
                
                // Đảm bảo currentPage hợp lệ
                if (currentPage >= totalPages)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;
                    
                // Lấy danh sách nghệ sĩ cho trang hiện tại
                var pageArtists = artists
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
                    
                // Hiển thị danh sách nghệ sĩ cho trang hiện tại
                DisplayArtists(pageArtists, currentSelection);
                
                // Hiển thị thông tin phân trang
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages} - Showing {pageArtists.Count} of {artists.Count} artists");
                
                // Hiển thị nghệ sĩ được chọn
                if (currentSelection >= pageArtists.Count)
                    currentSelection = pageArtists.Count - 1;
                    
                if (!isSearching && pageArtists.Count > 0 && currentSelection >= 0)
                {
                    Console.WriteLine("\nSelected artist:");
                    Artist selectedArtist = pageArtists[currentSelection];
                    Console.WriteLine($"ID: {selectedArtist.ArtistId}, Name: {selectedArtist.ArtistName}, Top Song: {selectedArtist.TopSong}");
                }
            }
            
            // Hiển thị thanh tìm kiếm nếu đang tìm kiếm
            if (isSearching)
            {
                Console.Write($"\nSearch: {searchTerm}");
            }
            else
            {
                // Hiển thị hướng dẫn
                Console.WriteLine("\nControls:");
                Console.WriteLine("↑↓: Navigate artists • ←→: Change pages • S: Search • D: Delete artist • Enter: Options • Esc: Back");
            }
            
            // Xử lý input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
            if (isSearching)
            {
                // Xử lý input khi đang tìm kiếm
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isSearching = false;
                    currentSelection = 0;
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isSearching = false;
                    searchTerm = ""; // Clear search
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTerm.Length > 0)
                {
                    searchTerm = searchTerm[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTerm += keyInfo.KeyChar;
                }
            }
            else
            {
                // Xử lý input khi không tìm kiếm
                var pageArtists = artists.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                            currentSelection--;
                        break;
                        
                    case ConsoleKey.DownArrow:
                        if (currentSelection < pageArtists.Count - 1)
                            currentSelection++;
                        break;
                        
                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        int totalPages = (int)Math.Ceiling(artists.Count / (double)itemsPerPage);
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.S:
                        isSearching = true;
                        searchTerm = "";
                        currentPage = 0; // Reset về trang đầu khi tìm kiếm
                        break;
                        
                    case ConsoleKey.D:
                        if (pageArtists.Count > 0 && currentSelection >= 0 && currentSelection < pageArtists.Count)
                        {
                            // Xóa nghệ sĩ được chọn
                            Artist selectedArtist = pageArtists[currentSelection];
                            DeleteSelectedArtist(selectedArtist, artistService);
                        }
                        break;
                        
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                        
                    case ConsoleKey.Enter:
                        if (pageArtists.Count > 0 && currentSelection >= 0 && currentSelection < pageArtists.Count)
                        {
                            // Hiển thị chi tiết và tùy chọn cho nghệ sĩ được chọn
                            Artist selectedArtist = pageArtists[currentSelection];
                            ShowArtistOptions(selectedArtist, artistService);
                        }
                        break;
                }
            }
        }
    }

    // Hiển thị tùy chọn cho nghệ sĩ đã chọn
    static void ShowArtistOptions(Artist artist, ArtistService artistService)
    {
        string[] options = { "Delete Artist", "Back" };
        int choice = ShowMenu($"Artist: {artist.ArtistName}", options);
        
        switch (choice)
        {              
            case 1:
                // Xóa nghệ sĩ
                DeleteSelectedArtist(artist, artistService);
                break;
        }
    }

    // Xóa nghệ sĩ đã chọn
    static void DeleteSelectedArtist(Artist artist, ArtistService artistService)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete artist '{artist.ArtistName}'?");
        Console.WriteLine("This action cannot be undone and will remove all songs by this artist.");
        Console.WriteLine("\nPress Enter to confirm, any other key to cancel...");
        
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            // Sử dụng phương thức AdminDeleteArtist từ ArtistService
            if (artistService.AdminDeleteArtist(artist.ArtistId))
            {
                Console.WriteLine("\nArtist deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete artist!");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    static void ManageSongs(SongService songService)
    {
        bool running = true;
        int currentSelection = 0;
        string searchTerm = "";
        bool isSearching = false;
        int currentPage = 0;
        int itemsPerPage = 10; // 10 bài hát mỗi trang
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Manage Songs ===\n");
            
            // Lấy danh sách bài hát từ service
            List<Song> songs;
            if (string.IsNullOrEmpty(searchTerm))
                songs = songService.GetAllSongs();
            else
                songs = songService.SearchSongs(searchTerm); // Sử dụng SearchSongs đã có
                        
            if (songs.Count == 0)
            {
                Console.WriteLine(string.IsNullOrEmpty(searchTerm) 
                    ? "No songs found!" 
                    : $"No songs found matching '{searchTerm}'");
            }
            else
            {
                // Tính toán số trang
                int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
                
                // Đảm bảo currentPage hợp lệ
                if (currentPage >= totalPages)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;
                    
                // Lấy danh sách bài hát cho trang hiện tại
                var pageSongs = songs
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
                    
                // Hiển thị danh sách bài hát cho trang hiện tại
                DisplaySongs(pageSongs, currentSelection);
                
                // Hiển thị thông tin phân trang
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages} - Showing {pageSongs.Count} of {songs.Count} songs");
                
                // Hiển thị bài hát được chọn
                if (currentSelection >= pageSongs.Count)
                    currentSelection = pageSongs.Count - 1;
                    
                if (!isSearching && pageSongs.Count > 0 && currentSelection >= 0)
                {
                    Console.WriteLine("\nSelected song:");
                    Song selectedSong = pageSongs[currentSelection];
                    Console.WriteLine($"ID: {selectedSong.SongId}, Title: {selectedSong.Title}, Artist: {selectedSong.ArtistName}, Album: {selectedSong.Album}, Genre: {selectedSong.Genre}");
                }
            }
            
            // Hiển thị thanh tìm kiếm nếu đang tìm kiếm
            if (isSearching)
            {
                Console.Write($"\nSearch: {searchTerm}");
            }
            else
            {
                // Hiển thị hướng dẫn
                Console.WriteLine("\nControls:");
                Console.WriteLine("↑↓: Navigate songs • ←→: Change pages • S: Search • D: Delete song • Enter: Options • Esc: Back");
            }
            
            // Xử lý input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
            if (isSearching)
            {
                // Xử lý input khi đang tìm kiếm
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isSearching = false;
                    currentSelection = 0;
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isSearching = false;
                    searchTerm = ""; // Clear search
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTerm.Length > 0)
                {
                    searchTerm = searchTerm[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTerm += keyInfo.KeyChar;
                }
            }
            else
            {
                // Xử lý input khi không tìm kiếm
                var pageSongs = songs.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                            currentSelection--;
                        break;
                        
                    case ConsoleKey.DownArrow:
                        if (currentSelection < pageSongs.Count - 1)
                            currentSelection++;
                        break;
                    
                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.S:
                        isSearching = true;
                        searchTerm = "";
                        currentPage = 0; // Reset về trang đầu khi tìm kiếm
                        break;
                        
                    case ConsoleKey.D:
                        if (pageSongs.Count > 0 && currentSelection >= 0 && currentSelection < pageSongs.Count)
                        {
                            // Xóa bài hát được chọn
                            Song selectedSong = pageSongs[currentSelection];
                            DeleteSelectedSong(selectedSong, songService);
                        }
                        break;
                        
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                        
                    case ConsoleKey.Enter:
                        if (pageSongs.Count > 0 && currentSelection >= 0 && currentSelection < pageSongs.Count)
                        {
                            // Hiển thị chi tiết và tùy chọn cho bài hát được chọn
                            Song selectedSong = pageSongs[currentSelection];
                            ShowSongOptions(selectedSong, songService);
                        }
                        break;
                }
            }
        }
    }

    // Hiển thị tùy chọn cho bài hát đã chọn
    static void ShowSongOptions(Song song, SongService songService)
    {
        string[] options = { "Delete Song", "Back" };
        int choice = ShowMenu($"Song: {song.Title}", options);
        
        switch (choice)
        {              
            case 1:
                // Xóa bài hát
                DeleteSelectedSong(song, songService);
                break;
        }
    }

    // Xóa bài hát đã chọn
    static void DeleteSelectedSong(Song song, SongService songService)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete song '{song.Title}' by {song.ArtistName}?");
        Console.WriteLine("This action cannot be undone.");
        Console.WriteLine("\nPress Enter to confirm, any other key to cancel...");
        
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            // Sử dụng phương thức DeleteArtistSong với quyền admin
            if (songService.DeleteArtistSong(song.SongId, 0, "admin")) // Admin có quyền xóa mọi bài hát
            {
                Console.WriteLine("\nSong deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete song!");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    static void DisplayUsers(List<User> users, int currentSelection)
    {
        // Tính toán chiều rộng cột
        int idWidth = 5;
        int nameWidth = Math.Max("Username".Length, users.Max(u => u.UserName?.Length ?? 0));
        int emailWidth = Math.Max("Email".Length, users.Max(u => u.UserEmail?.Length ?? 0));
        int roleWidth = Math.Max("Role".Length, users.Max(u => u.Roles?.Length ?? 0));
        
        // Thêm padding
        nameWidth += 2;
        emailWidth += 2;
        roleWidth += 2;
        
        // Tạo khung bảng
        string horizontalLine = $"╔═{new string('═', idWidth)}═╦═{new string('═', nameWidth)}═╦═{new string('═', emailWidth)}═╦═{new string('═', roleWidth)}═╗";
        string headerLine = $"║ {"ID".PadRight(idWidth)} ║ {"Username".PadRight(nameWidth)} ║ {"Email".PadRight(emailWidth)} ║ {"Role".PadRight(roleWidth)} ║";
        string separatorLine = $"╠═{new string('═', idWidth)}═╬═{new string('═', nameWidth)}═╬═{new string('═', emailWidth)}═╬═{new string('═', roleWidth)}═╣";
        string footerLine = $"╚═{new string('═', idWidth)}═╩═{new string('═', nameWidth)}═╩═{new string('═', emailWidth)}═╩═{new string('═', roleWidth)}═╝";
        
        Console.WriteLine(horizontalLine);
        Console.WriteLine(headerLine);
        Console.WriteLine(separatorLine);
        
        for (int i = 0; i < users.Count; i++)
        {
            string rowContent = $"║ {users[i].UserId.ToString().PadRight(idWidth)} ║ " +
                    $"{users[i].UserName.PadRight(nameWidth)} ║ " +
                    $"{users[i].UserEmail.PadRight(emailWidth)} ║ " +
                    $"{users[i].Roles.PadRight(roleWidth)} ║";
            
            // Highlight dòng được chọn - chỉ highlight nội dung trong bảng
            if (i == currentSelection)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(rowContent);
                Console.ResetColor(); // Reset ngay sau khi in xong nội dung
                Console.WriteLine(); // Xuống dòng sau khi đã reset màu
            }
            else
            {
                Console.WriteLine(rowContent);
            }
        }
        
        Console.WriteLine(footerLine);
    }

    static void DisplayArtists(List<Artist> artists, int currentSelection = -1)
    {
        // Tính toán chiều rộng cột
        int idWidth = 5;
        int nameWidth = Math.Max("Artist Name".Length, artists.Max(a => a.ArtistName?.Length ?? 0));
        int songWidth = Math.Max("Top Song".Length, artists.Max(a => a.TopSong?.Length ?? 0));
        
        // Thêm padding
        nameWidth += 2;
        songWidth += 2;
        
        // Tạo khung bảng
        string horizontalLine = $"╔═{new string('═', idWidth)}═╦═{new string('═', nameWidth)}═╦═{new string('═', songWidth)}═╗";
        string headerLine = $"║ {"ID".PadRight(idWidth)} ║ {"Artist Name".PadRight(nameWidth)} ║ {"Top Song".PadRight(songWidth)} ║";
        string separatorLine = $"╠═{new string('═', idWidth)}═╬═{new string('═', nameWidth)}═╬═{new string('═', songWidth)}═╣";
        string footerLine = $"╚═{new string('═', idWidth)}═╩═{new string('═', nameWidth)}═╩═{new string('═', songWidth)}═╝";
        
        Console.WriteLine(horizontalLine);
        Console.WriteLine(headerLine);
        Console.WriteLine(separatorLine);
        
        for (int i = 0; i < artists.Count; i++)
        {
            string rowContent = $"║ {artists[i].ArtistId.ToString().PadRight(idWidth)} ║ " +
                    $"{artists[i].ArtistName.PadRight(nameWidth)} ║ " +
                    $"{artists[i].TopSong.PadRight(songWidth)} ║";
            
            // Highlight dòng được chọn - chỉ highlight nội dung trong bảng
            if (i == currentSelection)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(rowContent);
                Console.ResetColor(); // Reset ngay sau khi in xong nội dung
                Console.WriteLine(); // Xuống dòng sau khi đã reset màu
            }
            else
            {
                Console.WriteLine(rowContent);
            }
        }
        
        Console.WriteLine(footerLine);
    }

    static void DisplaySongs(List<Song> songs, int currentSelection = -1)
    {
        // Calculate column widths - remove idWidth
        int idWidth = 5;
        int titleWidth = Math.Max("Title".Length, 
                            Math.Max(20, songs.Max(s => s.Title?.Length ?? 0)));
        int artistWidth = Math.Max("Artist".Length,
                            Math.Max(20, songs.Max(s => s.ArtistName?.Length ?? 0)));
        int albumWidth = Math.Max("Album".Length,
                            Math.Max(20, songs.Max(s => s.Album?.Length ?? 0)));
        int genreWidth = Math.Max("Genre".Length,
                            Math.Max(15, songs.Max(s => s.Genre?.Length ?? 0)));
        int dateWidth = 10;  // Fixed for yyyy-MM-dd format

        // Create table format strings with ID column
        string horizontalLine = $"╔═{new string('═', idWidth)}═╦═{new string('═', titleWidth)}═╦═{new string('═', artistWidth)}═╦═{new string('═', albumWidth)}═╦═{new string('═', genreWidth)}═╦═{new string('═', dateWidth)}═╗";
        string headerLine =    $"║ {"ID".PadRight(idWidth)} ║ {"Title".PadRight(titleWidth)} ║ {"Artist".PadRight(artistWidth)} ║ {"Album".PadRight(albumWidth)} ║ {"Genre".PadRight(genreWidth)} ║ {"Release".PadRight(dateWidth)} ║";
        string separatorLine = $"╠═{new string('═', idWidth)}═╬═{new string('═', titleWidth)}═╬═{new string('═', artistWidth)}═╬═{new string('═', albumWidth)}═╬═{new string('═', genreWidth)}═╬═{new string('═', dateWidth)}═╣";
        string footerLine =    $"╚═{new string('═', idWidth)}═╩═{new string('═', titleWidth)}═╩═{new string('═', artistWidth)}═╩═{new string('═', albumWidth)}═╩═{new string('═', genreWidth)}═╩═{new string('═', dateWidth)}═╝";

        Console.WriteLine(horizontalLine);
        Console.WriteLine(headerLine);
        Console.WriteLine(separatorLine);

        for (int i = 0; i < songs.Count; i++)
        {
            string formattedDate = songs[i].ReleaseDate?.ToString("yyyy-MM-dd") ?? "N/A";
            string rowContent = $"║ {songs[i].SongId.ToString().PadRight(idWidth)} ║ " +
                        $"{songs[i].Title?.PadRight(titleWidth)} ║ " +
                        $"{songs[i].ArtistName?.PadRight(artistWidth)} ║ " +
                        $"{songs[i].Album?.PadRight(albumWidth)} ║ " +
                        $"{songs[i].Genre?.PadRight(genreWidth)} ║ " +
                        $"{formattedDate.PadRight(dateWidth)} ║";
                        
            // Highlight dòng được chọn - chỉ highlight nội dung trong bảng
            if (i == currentSelection)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(rowContent);
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(rowContent);
            }
        }

        Console.WriteLine(footerLine);
    }

    static void DisplayPlaylists(List<Playlist> playlists, int currentSelection = -1)
    {
        // Calculate max widths without ID column
        int idWidth = 5;
        int nameWidth = Math.Max("Name".Length, 
                                playlists.Max(p => p.PlaylistName.Length));
        int userWidth = Math.Max("Creator".Length,
                                playlists.Max(p => p.Username?.Length ?? 0));
        int dateWidth = Math.Max("Created Date".Length, 10); // "yyyy-MM-dd"

        // Add padding to make columns look better
        nameWidth += 4;
        userWidth += 4;
        dateWidth += 4;

        // Create borders with exact spacing
        string header =    $"╔═{new string('═', idWidth)}═╦═{new string('═', nameWidth)}═╦═{new string('═', userWidth)}═╦═{new string('═', dateWidth)}═╗";
        string titleRow =  $"║ {"ID".PadRight(idWidth)} ║ {"Name".PadRight(nameWidth)} ║ {"Creator".PadRight(userWidth)} ║ {"Created".PadRight(dateWidth)} ║";
        string separator = $"╠═{new string('═', idWidth)}═╬═{new string('═', nameWidth)}═╬═{new string('═', userWidth)}═╬═{new string('═', dateWidth)}═╣";
        string footer =    $"╚═{new string('═', idWidth)}═╩═{new string('═', nameWidth)}═╩═{new string('═', userWidth)}═╩═{new string('═', dateWidth)}═╝";

        Console.WriteLine(header);
        Console.WriteLine(titleRow);
        Console.WriteLine(separator);
        
        for (int i = 0; i < playlists.Count; i++)
        {
            string rowContent = $"║ {playlists[i].PlaylistId.ToString().PadRight(idWidth)} ║ " +
                            $"{playlists[i].PlaylistName.PadRight(nameWidth)} ║ " +
                            $"{playlists[i].Username?.PadRight(userWidth) ?? "Unknown".PadRight(userWidth)} ║ " +
                            $"{playlists[i].CreatedAt.ToString("yyyy-MM-dd").PadRight(dateWidth)} ║";
            
            // Highlight dòng được chọn - chỉ highlight nội dung trong bảng
            if (i == currentSelection)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(rowContent);
                Console.ResetColor(); // Reset ngay sau khi in xong nội dung
                Console.WriteLine(); // Xuống dòng sau khi đã reset màu
            }
            else
            {
                Console.WriteLine(rowContent);
            }
        }
        
        Console.WriteLine(footer);
    }

    static void ShowArtistMenu(User user)
    {
        Console.Clear();
        bool running = true;
        string[] options = { 
            "Register Information",
            "Upload Song",
            "My Songs",
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
                    UploadSong(songService, user, artistService);
                    break;
                case 3:
                    ViewMySongs(songService, artistService, user);
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

    static void RegisterArtist(ArtistService artistService, User currentUser)
    {
        Console.Clear();
        Console.WriteLine("=== Artist Registration ===\n");

        // Kiểm tra xem người dùng đã đăng ký làm nghệ sĩ chưa
        if (artistService.IsArtistRegistered(currentUser.UserId))
        {
            Console.WriteLine("You have already registered as an artist!");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        // Nhập tên nghệ sĩ
        string name = "";
        while (true)
        {
            Console.Write("Artist Name: ");
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(name)) break;
                }
                else if (key.Key == ConsoleKey.Backspace && name.Length > 0)
                {
                    name = name[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    name += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(name)) break;
        }

        // Nhập ngày sinh với kiểm tra định dạng
        DateTime birthDate;
        string dateInput = "";
        while (true)
        {
            Console.Write("Birth Date (YYYY-MM-DD): ");
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && dateInput.Length > 0)
                {
                    dateInput = dateInput[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    dateInput += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            
            if (DateTime.TryParse(dateInput, out birthDate))
            {
                // Kiểm tra tuổi hợp lệ (ví dụ: ít nhất 13 tuổi)
                if (DateTime.Now.Year - birthDate.Year >= 13) break;
                Console.WriteLine("You must be at least 13 years old to register as an artist.");
            }
            else
            {
                Console.WriteLine("Invalid date format. Please use YYYY-MM-DD format.");
            }
        }

        // Nhập bài hát nổi tiếng nhất
        string topSong = "";
        while (true)
        {
            Console.Write("Top Song: ");
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(topSong)) break;
                }
                else if (key.Key == ConsoleKey.Backspace && topSong.Length > 0)
                {
                    topSong = topSong[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    topSong += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(topSong)) break;
        }

        // Hiển thị xác nhận
        Console.Clear();
        Console.WriteLine("=== Artist Registration ===\n");
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Birth Date: {birthDate:yyyy-MM-dd}");
        Console.WriteLine($"Top Song: {topSong}");
        Console.WriteLine("\nConfirm registration? (Y/N): ");

        if (Console.ReadKey(true).Key == ConsoleKey.Y)
        {
            // Tạo đối tượng Artist để truyền vào phương thức RegisterArtist
            Artist artistInfo = new Artist
            {
                ArtistName = name,
                BirthDate = birthDate,
                TopSong = topSong,
                UserId = currentUser.UserId
            };
            
            // Đăng ký thông tin nghệ sĩ bằng cách gọi service
            bool success = artistService.RegisterArtist(artistInfo);
            
            if (success)
            {
                Console.WriteLine("\nArtist registration successful!");
            }
            else
            {
                Console.WriteLine("\nArtist registration failed. Please try again later.");
            }
        }
        else
        {
            Console.WriteLine("\nRegistration cancelled.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void UploadSong(SongService songService, User currentUser, ArtistService artistService)
    {
        Console.Clear();
        Console.WriteLine("=== Upload Song ===\n");

        // Kiểm tra xem người dùng đã đăng ký làm nghệ sĩ chưa
        var artist = artistService.GetArtistByUserId(currentUser.UserId);
        if (artist == null)
        {
            Console.WriteLine("You need to register as an artist before uploading songs.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        // Title input
        string title = "";
        while (true)
        {
            Console.Write("Song Title: ");
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

        // Album input
        string album = "";
        while (true)
        {
            Console.Write("Album: ");
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
            Console.Write("Genre: ");
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
        string dateInput = "";
        while (true)
        {
            Console.Write("Release Date (YYYY-MM-DD): ");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && dateInput.Length > 0)
                {
                    dateInput = dateInput[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    dateInput += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            
            // Kiểm tra ngày hợp lệ
            if (DateTime.TryParse(dateInput, out releaseDate))
            {
                // Không cho phép ngày phát hành trong tương lai
                if (releaseDate <= DateTime.Now)
                    break;
                else
                    Console.WriteLine("Release date cannot be in the future.");
            }
            else
            {
                Console.WriteLine("Invalid date format. Please use YYYY-MM-DD format.");
            }
        }

        // Hiển thị xác nhận
        Console.Clear();
        Console.WriteLine("=== Upload Song ===\n");
        Console.WriteLine($"Title: {title}");
        Console.WriteLine($"Artist: {artist.ArtistName}");
        Console.WriteLine($"Album: {album}");
        Console.WriteLine($"Genre: {genre}");
        Console.WriteLine($"Release Date: {releaseDate:yyyy-MM-dd}");
        Console.WriteLine("\nConfirm upload? (Y/N): ");

        if (Console.ReadKey(true).Key == ConsoleKey.Y)
        {          
            // Upload bài hát bằng cách gọi service
            bool success = songService.AddArtistSong(title, artist.ArtistId, album, genre, releaseDate);
            
            if (success)
            {
                Console.WriteLine("\nSong uploaded successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to upload song. Please try again later.");
            }
        }
        else
        {
            Console.WriteLine("\nUpload cancelled.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void ViewMySongs(SongService songService, ArtistService artistService, User currentUser)
    {
        Console.Clear();
        Console.WriteLine("=== My Songs ===\n");
        
        // Kiểm tra xem người dùng đã đăng ký làm nghệ sĩ chưa
        var artist = artistService.GetArtistByUserId(currentUser.UserId);
        if (artist == null)
        {
            Console.WriteLine("You need to register as an artist first!");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        bool running = true;
        int currentSelection = 0;
        string searchTerm = "";
        bool isSearching = false;
        int currentPage = 0;
        int itemsPerPage = 10; // 10 bài hát mỗi trang
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== My Songs ===\n");
            
            // Lấy danh sách bài hát của nghệ sĩ
            List<Song> songs;
            if (string.IsNullOrEmpty(searchTerm))
                songs = songService.GetArtistSongs(artist.ArtistId);
            else
                songs = songService.GetArtistSongs(artist.ArtistId)
                        .Where(s => s.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            s.Album.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            s.Genre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                        
            if (songs.Count == 0)
            {
                Console.WriteLine(string.IsNullOrEmpty(searchTerm) 
                    ? "You haven't uploaded any songs yet!" 
                    : $"No songs found matching '{searchTerm}'");
            }
            else
            {
                // Tính toán số trang
                int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
                
                // Đảm bảo currentPage hợp lệ
                if (currentPage >= totalPages)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;
                    
                // Lấy danh sách bài hát cho trang hiện tại
                var pageSongs = songs
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
                    
                // Hiển thị danh sách bài hát cho trang hiện tại
                DisplaySongs(pageSongs, currentSelection);
                
                // Hiển thị thông tin phân trang
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages} - Showing {pageSongs.Count} of {songs.Count} songs");
                
                // Hiển thị bài hát được chọn
                if (currentSelection >= pageSongs.Count)
                    currentSelection = pageSongs.Count - 1;
                    
                if (!isSearching && pageSongs.Count > 0 && currentSelection >= 0)
                {
                    Console.WriteLine("\nSelected song:");
                    Song selectedSong = pageSongs[currentSelection];
                    Console.WriteLine($"ID: {selectedSong.SongId}, Title: {selectedSong.Title}, Album: {selectedSong.Album}, Genre: {selectedSong.Genre}, Release Date: {selectedSong.ReleaseDate:yyyy-MM-dd}");
                }
            }
            
            // Hiển thị thanh tìm kiếm nếu đang tìm kiếm
            if (isSearching)
            {
                Console.Write($"\nSearch: {searchTerm}");
            }
            else
            {
                // Hiển thị hướng dẫn
                Console.WriteLine("\nControls:");
                Console.WriteLine("↑↓: Navigate songs • ←→: Change pages • S: Search • D: Delete song • Enter: Song options • Esc: Back");
            }
            
            // Xử lý input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
            if (isSearching)
            {
                // Xử lý input khi đang tìm kiếm
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isSearching = false;
                    currentSelection = 0;
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isSearching = false;
                    searchTerm = ""; // Clear search
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTerm.Length > 0)
                {
                    searchTerm = searchTerm[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTerm += keyInfo.KeyChar;
                }
            }
            else
            {
                // Xử lý input khi không tìm kiếm
                var pageSongs = songs.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                            currentSelection--;
                        break;
                        
                    case ConsoleKey.DownArrow:
                        if (currentSelection < pageSongs.Count - 1)
                            currentSelection++;
                        break;
                    
                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.S:
                        isSearching = true;
                        searchTerm = "";
                        currentPage = 0; // Reset về trang đầu khi tìm kiếm
                        break;
                        
                    case ConsoleKey.D:
                        if (pageSongs.Count > 0 && currentSelection >= 0 && currentSelection < pageSongs.Count)
                        {
                            // Xóa bài hát được chọn
                            Song selectedSong = pageSongs[currentSelection];
                            DeleteArtistSong(selectedSong, songService, artist.ArtistId);
                        }
                        break;
                        
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                        
                    case ConsoleKey.Enter:
                        if (pageSongs.Count > 0 && currentSelection >= 0 && currentSelection < pageSongs.Count)
                        {
                            // Hiển thị chi tiết và tùy chọn cho bài hát được chọn
                            Song selectedSong = pageSongs[currentSelection];
                            ShowArtistSongOptions(selectedSong, songService, artist.ArtistId);
                        }
                        break;
                }
            }
        }
    }

    // Hiển thị tùy chọn cho bài hát của nghệ sĩ
    static void ShowArtistSongOptions(Song song, SongService songService, int artistId)
    {
        string[] options = { "Delete Song", "Back" };
        int choice = ShowMenu($"Song: {song.Title}", options);
        
        switch (choice)
        {              
            case 1:
                // Xóa bài hát
                DeleteArtistSong(song, songService, artistId);
                break;
        }
    }

    // Xóa bài hát của nghệ sĩ
    static void DeleteArtistSong(Song song, SongService songService, int artistId)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete your song '{song.Title}'?");
        Console.WriteLine("This action cannot be undone.");
        Console.WriteLine("\nPress Enter to confirm, any other key to cancel...");
        
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            // Sử dụng phương thức DeleteArtistSong với quyền artistId
            if (songService.DeleteArtistSong(song.SongId, artistId, "artist"))
            {
                Console.WriteLine("\nSong deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete song. You can only delete your own songs.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    static void ShowListenerMenu(User user)
    {
        bool running = true;
        string[] options = { 
            "Browse songs",
            "Create playlist",
            "View playlists",
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
                    CreatePlaylist(playlistService, user);
                    break;
                case 3:
                    ViewMyPlaylists(playlistService, songService, user);
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
        Console.Clear();
        Console.WriteLine("=== Browse Songs ===\n");
        
        bool running = true;
        int currentSelection = 0;
        string searchTerm = "";
        bool isSearching = false;
        int currentPage = 0;
        int itemsPerPage = 10; // 10 bài hát mỗi trang
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Browse Songs ===\n");
            
            // Lấy danh sách bài hát
            List<Song> songs;
            if (string.IsNullOrEmpty(searchTerm))
                songs = songService.GetAllSongs();
            else
                songs = songService.SearchSongs(searchTerm);
                        
            if (songs.Count == 0)
            {
                Console.WriteLine(string.IsNullOrEmpty(searchTerm) 
                    ? "No songs found in the library!" 
                    : $"No songs found matching '{searchTerm}'");
            }
            else
            {
                // Tính toán số trang
                int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
                
                // Đảm bảo currentPage hợp lệ
                if (currentPage >= totalPages)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;
                    
                // Lấy danh sách bài hát cho trang hiện tại
                var pageSongs = songs
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
                    
                // Hiển thị danh sách bài hát cho trang hiện tại
                DisplaySongs(pageSongs, currentSelection);
                
                // Hiển thị thông tin phân trang
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages} - Showing {pageSongs.Count} of {songs.Count} songs");
                
                // Hiển thị bài hát được chọn
                if (currentSelection >= pageSongs.Count)
                    currentSelection = pageSongs.Count - 1;
                    
                if (!isSearching && pageSongs.Count > 0 && currentSelection >= 0)
                {
                    Console.WriteLine("\nSelected song:");
                    Song selectedSong = pageSongs[currentSelection];
                    Console.WriteLine($"Title: {selectedSong.Title}, Artist: {selectedSong.ArtistName}, Album: {selectedSong.Album}, Genre: {selectedSong.Genre}");
                }
            }
            
            // Hiển thị thanh tìm kiếm nếu đang tìm kiếm
            if (isSearching)
            {
                Console.Write($"\nSearch: {searchTerm}");
            }
            else
            {
                // Hiển thị hướng dẫn
                Console.WriteLine("\nControls:");
                Console.WriteLine("↑↓: Navigate songs • ←→: Change pages • S: Search • Enter: Add to playlist • Esc: Back");
            }
            
            // Xử lý input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
            if (isSearching)
            {
                // Xử lý input khi đang tìm kiếm
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isSearching = false;
                    currentSelection = 0;
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isSearching = false;
                    searchTerm = ""; // Clear search
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTerm.Length > 0)
                {
                    searchTerm = searchTerm[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTerm += keyInfo.KeyChar;
                }
            }
            else
            {
                // Xử lý input khi không tìm kiếm
                var pageSongs = songs.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                            currentSelection--;
                        break;
                        
                    case ConsoleKey.DownArrow:
                        if (currentSelection < pageSongs.Count - 1)
                            currentSelection++;
                        break;
                    
                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            currentSelection = 0; // Reset selection khi chuyển trang
                        }
                        break;
                        
                    case ConsoleKey.S:
                        isSearching = true;
                        searchTerm = "";
                        currentPage = 0; // Reset về trang đầu khi tìm kiếm
                        break;
                        
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                        
                    case ConsoleKey.Enter:
                        if (pageSongs.Count > 0 && currentSelection >= 0 && currentSelection < pageSongs.Count)
                        {
                            // Hiển thị tùy chọn thêm bài hát vào playlist
                            Song selectedSong = pageSongs[currentSelection];
                            AddSongToPlaylist(selectedSong, playlistService, user);
                        }
                        break;
                }
            }
        }
    }

    // Phương thức để thêm bài hát vào playlist
    static void AddSongToPlaylist(Song song, PlaylistService playlistService, User user)
    {
        Console.Clear();
        Console.WriteLine($"=== Add Song to Playlist ===\n");
        Console.WriteLine($"Song: {song.Title} by {song.ArtistName}");
        
        // Lấy danh sách playlist của user
        var userPlaylists = playlistService.GetUserPlaylists(user.UserId);
        
        if (userPlaylists.Count == 0)
        {
            Console.WriteLine("\nYou don't have any playlists yet!");
            Console.WriteLine("Would you like to create a new playlist? (Y/N): ");
            
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                CreatePlaylist(playlistService, user);
                // Lấy lại danh sách sau khi tạo mới
                userPlaylists = playlistService.GetUserPlaylists(user.UserId);
                if (userPlaylists.Count == 0)
                {
                    // Vẫn không có playlist, có thể đã xảy ra lỗi
                    Console.WriteLine("\nFailed to create playlist. Please try again later.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                return;
            }
        }
        
        Console.WriteLine("\nChoose a playlist to add this song to:\n");
        
        int currentSelection = 0;
        ConsoleKey key;
        
        do
        {
            Console.Clear();
            Console.WriteLine($"=== Add Song to Playlist ===\n");
            Console.WriteLine($"Song: {song.Title} by {song.ArtistName}\n");
            Console.WriteLine("Choose a playlist:\n");
            
            for (int i = 0; i < userPlaylists.Count; i++)
            {
                if (i == currentSelection)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"► {userPlaylists[i].PlaylistName}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {userPlaylists[i].PlaylistName}");
                }
            }
            
            Console.WriteLine("\nUse ↑↓ to navigate, Enter to select, Esc to cancel");
            
            key = Console.ReadKey(true).Key;
            
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (currentSelection > 0)
                        currentSelection--;
                    break;
                    
                case ConsoleKey.DownArrow:
                    if (currentSelection < userPlaylists.Count - 1)
                        currentSelection++;
                    break;
            }
        } while (key != ConsoleKey.Enter && key != ConsoleKey.Escape);
        
        if (key == ConsoleKey.Escape)
            return;
        
        // Người dùng đã chọn playlist
        Playlist selectedPlaylist = userPlaylists[currentSelection];
        
        // Thêm bài hát vào playlist
        bool success = playlistService.AddSongToUserPlaylist(selectedPlaylist.PlaylistId, song.SongId, user.UserId);
        
        Console.Clear();
        Console.WriteLine($"=== Add Song to Playlist ===\n");
        
        if (success)
        {
            Console.WriteLine($"Successfully added '{song.Title}' to playlist '{selectedPlaylist.PlaylistName}'!");
        }
        else
        {
            Console.WriteLine("Failed to add song to playlist. It may already be in the playlist or an error occurred.");
        }
        
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void CreatePlaylist(PlaylistService playlistService, User user)
    {
        Console.Clear();
        Console.WriteLine("=== Create New Playlist ===\n");
        
        string playlistName = "";
        while (string.IsNullOrWhiteSpace(playlistName))
        {
            Console.Write("Enter playlist name: ");
            ConsoleKeyInfo key;
            
            // Đọc tên playlist với hỗ trợ backspace và escape
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) 
                {
                    return; // Thoát nếu nhấn Escape
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break; // Hoàn thành nhập liệu khi nhấn Enter
                }
                else if (key.Key == ConsoleKey.Backspace && playlistName.Length > 0)
                {
                    playlistName = playlistName[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    playlistName += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            Console.WriteLine();
            
            // Nếu tên playlist vẫn trống, yêu cầu người dùng nhập lại
            if (string.IsNullOrWhiteSpace(playlistName))
            {
                Console.WriteLine("Playlist name cannot be empty. Please enter a valid name.");
            }
        }
        
        // Gọi service để tạo playlist
        bool success = playlistService.CreateUserPlaylist(playlistName, user.UserId);
        
        // Hiển thị kết quả
        if (success)
        {
            Console.WriteLine("\nPlaylist created successfully!");
        }
        else
        {
            Console.WriteLine("\nFailed to create playlist. Please try again later.");
        }
        
        // Đợi người dùng nhấn phím bất kỳ trước khi trở về menu
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void ViewMyPlaylists(PlaylistService playlistService, SongService songService, User user)
    {
        Console.Clear();
        Console.WriteLine("=== My Playlists ===\n");
        
        bool running = true;
        int currentSelection = 0;
        string searchTerm = "";
        bool isSearching = false;
        int currentPage = 0;
        int itemsPerPage = 10; // 10 playlist mỗi trang
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== My Playlists ===\n");
            
            // Lấy danh sách playlist của user
            List<Playlist> playlists;
            if (string.IsNullOrEmpty(searchTerm))
                playlists = playlistService.GetUserPlaylists(user.UserId);
            else
                playlists = playlistService.GetUserPlaylists(user.UserId)
                            .Where(p => p.PlaylistName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        
            if (playlists.Count == 0)
            {
                Console.WriteLine(string.IsNullOrEmpty(searchTerm) 
                    ? "You don't have any playlists yet!" 
                    : $"No playlists found matching '{searchTerm}'");
                
                if (string.IsNullOrEmpty(searchTerm))
                {
                    Console.WriteLine("\nWould you like to create a new playlist? (Y/N)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        CreatePlaylist(playlistService, user);
                        // Reset và tiếp tục vòng lặp
                        searchTerm = "";
                        continue;
                    }
                    else
                    {
                        running = false;
                        continue;
                    }
                }
            }
            else
            {
                // Tính toán số trang
                int totalPages = (int)Math.Ceiling(playlists.Count / (double)itemsPerPage);
                
                // Đảm bảo currentPage hợp lệ
                if (currentPage >= totalPages)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;
                    
                // Lấy danh sách playlist cho trang hiện tại
                var pagePlaylists = playlists
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
                    
                // Hiển thị danh sách playlist cho trang hiện tại
                DisplayPlaylists(pagePlaylists, currentSelection);
                
                // Hiển thị thông tin phân trang
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages} - Showing {pagePlaylists.Count} of {playlists.Count} playlists");
                
                // Hiển thị playlist được chọn
                if (currentSelection >= pagePlaylists.Count)
                    currentSelection = pagePlaylists.Count - 1;
                    
                if (!isSearching && pagePlaylists.Count > 0 && currentSelection >= 0)
                {
                    Console.WriteLine("\nSelected playlist:");
                    Playlist selectedPlaylist = pagePlaylists[currentSelection];
                    Console.WriteLine($"Name: {selectedPlaylist.PlaylistName}, Created on: {selectedPlaylist.CreatedAt:yyyy-MM-dd}");
                }
            }
            
            // Hiển thị thanh tìm kiếm nếu đang tìm kiếm
            if (isSearching)
            {
                Console.Write($"\nSearch: {searchTerm}");
            }
            else
            {
                // Hiển thị hướng dẫn
                Console.WriteLine("\nControls:");
                Console.WriteLine("↑↓: Navigate playlists • ←→: Change pages • S: Search • D: Delete playlist • Enter: View songs • Esc: Back");
            }
            
            // Xử lý input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
            if (isSearching)
            {
                // Xử lý input khi đang tìm kiếm
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isSearching = false;
                    currentSelection = 0;
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isSearching = false;
                    searchTerm = ""; // Clear search
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTerm.Length > 0)
                {
                    searchTerm = searchTerm[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTerm += keyInfo.KeyChar;
                }
            }
            else
            {
                // Xử lý input khi không tìm kiếm
                if (playlists.Count > 0) // Chỉ xử lý nếu có playlist
                {
                    var pagePlaylists = playlists.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                    
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (currentSelection > 0)
                                currentSelection--;
                            break;
                            
                        case ConsoleKey.DownArrow:
                            if (currentSelection < pagePlaylists.Count - 1)
                                currentSelection++;
                            break;
                        
                        case ConsoleKey.LeftArrow:
                            if (currentPage > 0)
                            {
                                currentPage--;
                                currentSelection = 0; // Reset selection khi chuyển trang
                            }
                            break;
                            
                        case ConsoleKey.RightArrow:
                            int totalPages = (int)Math.Ceiling(playlists.Count / (double)itemsPerPage);
                            if (currentPage < totalPages - 1)
                            {
                                currentPage++;
                                currentSelection = 0; // Reset selection khi chuyển trang
                            }
                            break;
                            
                        case ConsoleKey.S:
                            isSearching = true;
                            searchTerm = "";
                            currentPage = 0; // Reset về trang đầu khi tìm kiếm
                            break;
                            
                        case ConsoleKey.D:
                            if (pagePlaylists.Count > 0 && currentSelection >= 0 && currentSelection < pagePlaylists.Count)
                            {
                                // Xóa playlist được chọn
                                Playlist selectedPlaylist = pagePlaylists[currentSelection];
                                DeleteUserPlaylist(selectedPlaylist, playlistService, user.UserId);
                            }
                            break;
                            
                        case ConsoleKey.Escape:
                            running = false;
                            break;
                            
                        case ConsoleKey.Enter:
                            if (pagePlaylists.Count > 0 && currentSelection >= 0 && currentSelection < pagePlaylists.Count)
                            {
                                // Xem danh sách bài hát trong playlist được chọn
                                Playlist selectedPlaylist = pagePlaylists[currentSelection];
                                ShowPlaylistOptions(selectedPlaylist, playlistService, songService, user);
                            }
                            break;
                    }
                }
                else // Nếu không có playlist nào
                {
                    if (keyInfo.Key == ConsoleKey.Escape)
                        running = false;
                    else if (keyInfo.Key == ConsoleKey.Y)
                    {
                        CreatePlaylist(playlistService, user);
                        searchTerm = "";
                    }
                }
            }
        }
    }

    static void ShowPlaylistOptions(Playlist playlist, PlaylistService playlistService, SongService songService, User user)
    {
        bool running = true;
        int currentSelection = 0;
        string[] options = { "View Songs", "Delete Playlist", "Back" };
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine($"=== Playlist: {playlist.PlaylistName} ===\n");
            Console.WriteLine("Choose an option:\n");
            
            for (int i = 0; i < options.Length; i++)
            {
                if (i == currentSelection)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"► {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }
            
            Console.WriteLine("\nUse ↑↓ to navigate, Enter to select, Esc to go back");
            
            ConsoleKeyInfo key = Console.ReadKey(true);
            
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (currentSelection > 0)
                        currentSelection--;
                    break;
                    
                case ConsoleKey.DownArrow:
                    if (currentSelection < options.Length - 1)
                        currentSelection++;
                    break;
                    
                case ConsoleKey.Escape:
                    running = false;
                    break;
                    
                case ConsoleKey.Enter:
                    switch (currentSelection)
                    {
                        case 0: // View Songs
                            ViewPlaylistSongs(playlist, playlistService, songService, user.UserId);
                            break;
                            
                        case 1: // Delete Playlist
                            DeleteUserPlaylist(playlist, playlistService, user.UserId);
                            // Sau khi xóa, cần thoát khỏi menu này
                            running = false;
                            break;
                            
                        case 2: // Back
                            running = false;
                            break;
                    }
                    break;
            }
        }
    }

    // Xóa playlist của người dùng
    // Xóa playlist của người dùng
    static void DeleteUserPlaylist(Playlist playlist, PlaylistService playlistService, int userId)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete playlist '{playlist.PlaylistName}'?");
        Console.WriteLine("This action cannot be undone and will remove all songs from this playlist.");
        Console.WriteLine("\nPress Enter to confirm, any other key to cancel...");
        
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            // Thêm debug info
            Console.WriteLine($"\nAttempting to delete playlist: {playlist.PlaylistName}");
            
            // Sử dụng phương thức DeletePlaylist với quyền userId và role là "listener"
            bool success = playlistService.DeletePlaylist(playlist.PlaylistId, userId, "listener");
            
            if (success)
            {
                Console.WriteLine("\nPlaylist deleted successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to delete playlist. You can only delete your own playlists.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    // Xem các bài hát trong playlist
    static void ViewPlaylistSongs(Playlist playlist, PlaylistService playlistService, SongService songService, int userId)
    {
        Console.Clear();
        Console.WriteLine($"=== Songs in '{playlist.PlaylistName}' ===\n");
        
        bool running = true;
        int currentSelection = 0;
        string searchTerm = "";
        bool isSearching = false;
        int currentPage = 0;
        int itemsPerPage = 10; // 10 bài hát mỗi trang
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine($"=== Songs in '{playlist.PlaylistName}' ===\n");
            
            // Lấy danh sách bài hát từ playlist
            List<Song> songs;
            if (string.IsNullOrEmpty(searchTerm))
                songs = songService.GetUserPlaylistSongs(playlist.PlaylistId, userId, "listener");
            else
                songs = songService.GetUserPlaylistSongs(playlist.PlaylistId, userId, "listener")
                        .Where(s => s.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            s.ArtistName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            s.Album.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                        
            if (songs.Count == 0)
            {
                Console.WriteLine(string.IsNullOrEmpty(searchTerm) 
                    ? "This playlist doesn't have any songs yet!" 
                    : $"No songs found matching '{searchTerm}'");
            }
            else
            {
                // Tính toán số trang
                int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
                
                // Đảm bảo currentPage hợp lệ
                if (currentPage >= totalPages)
                    currentPage = totalPages - 1;
                if (currentPage < 0)
                    currentPage = 0;
                    
                // Lấy danh sách bài hát cho trang hiện tại
                var pageSongs = songs
                    .Skip(currentPage * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
                    
                // Hiển thị danh sách bài hát cho trang hiện tại
                DisplaySongs(pageSongs, currentSelection);
                
                // Hiển thị thông tin phân trang
                Console.WriteLine($"\nPage {currentPage + 1}/{totalPages} - Showing {pageSongs.Count} of {songs.Count} songs");
                
                // Hiển thị bài hát được chọn
                if (currentSelection >= pageSongs.Count)
                    currentSelection = pageSongs.Count - 1;
                    
                if (!isSearching && pageSongs.Count > 0 && currentSelection >= 0)
                {
                    Console.WriteLine("\nSelected song:");
                    Song selectedSong = pageSongs[currentSelection];
                    Console.WriteLine($"Title: {selectedSong.Title}, Artist: {selectedSong.ArtistName}, Album: {selectedSong.Album}, Genre: {selectedSong.Genre}");
                }
            }
            
            // Hiển thị thanh tìm kiếm nếu đang tìm kiếm
            if (isSearching)
            {
                Console.Write($"\nSearch: {searchTerm}");
            }
            else
            {
                // Hiển thị hướng dẫn
                Console.WriteLine("\nControls:");
                Console.WriteLine("↑↓: Navigate songs • ←→: Change pages • S: Search • R: Remove song • Esc: Back");
            }
            
            // Xử lý input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            
            if (isSearching)
            {
                // Xử lý input khi đang tìm kiếm
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    isSearching = false;
                    currentSelection = 0;
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isSearching = false;
                    searchTerm = ""; // Clear search
                    currentPage = 0; // Reset về trang đầu
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && searchTerm.Length > 0)
                {
                    searchTerm = searchTerm[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    searchTerm += keyInfo.KeyChar;
                }
            }
            else
            {
                if (songs.Count > 0) // Chỉ xử lý nếu có bài hát
                {
                    var pageSongs = songs.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
                    
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (currentSelection > 0)
                                currentSelection--;
                            break;
                            
                        case ConsoleKey.DownArrow:
                            if (currentSelection < pageSongs.Count - 1)
                                currentSelection++;
                            break;
                        
                        case ConsoleKey.LeftArrow:
                            if (currentPage > 0)
                            {
                                currentPage--;
                                currentSelection = 0; // Reset selection khi chuyển trang
                            }
                            break;
                            
                        case ConsoleKey.RightArrow:
                            int totalPages = (int)Math.Ceiling(songs.Count / (double)itemsPerPage);
                            if (currentPage < totalPages - 1)
                            {
                                currentPage++;
                                currentSelection = 0; // Reset selection khi chuyển trang
                            }
                            break;
                            
                        case ConsoleKey.S:
                            isSearching = true;
                            searchTerm = "";
                            currentPage = 0; // Reset về trang đầu khi tìm kiếm
                            break;
                            
                        case ConsoleKey.R:
                            if (pageSongs.Count > 0 && currentSelection >= 0 && currentSelection < pageSongs.Count)
                            {
                                // Xóa bài hát khỏi playlist
                                Song selectedSong = pageSongs[currentSelection];
                                RemoveSongFromPlaylist(selectedSong, playlistService, playlist.PlaylistId, userId);
                            }
                            break;
                            
                        case ConsoleKey.Escape:
                            running = false;
                            break;
                    }
                }
                else // Nếu không có bài hát nào
                {
                    if (keyInfo.Key == ConsoleKey.Escape)
                        running = false;
                    else if (keyInfo.Key == ConsoleKey.S)
                    {
                        isSearching = true;
                        searchTerm = "";
                    }
                }
            }
        }
    }

    // Xóa bài hát khỏi playlist
    static void RemoveSongFromPlaylist(Song song, PlaylistService playlistService, int playlistId, int userId)
    {
        Console.Clear();
        Console.WriteLine($"Are you sure you want to remove '{song.Title}' by {song.ArtistName} from this playlist?");
        Console.WriteLine("\nPress Enter to confirm, any other key to cancel...");
        
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            // Sử dụng phương thức RemoveSongFromPlaylist 
            if (playlistService.DeleteUserPlaylistSong(playlistId, song.SongId, userId))
            {
                Console.WriteLine("\nSong removed from playlist successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to remove song from playlist. You can only modify your own playlists.");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}