using MySql.Data.MySqlClient;
using Persistence.Models;

namespace DAL
{
    public class PlaylistDAL
    {
        private readonly string connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=iLoveNOTP69;";

        public List<Playlist> GetUserPlaylists(int userId)
        {
            List<Playlist> playlists = new();
            using var conn = new MySqlConnection(connectionString);
            string query = "SELECT p.*, u.username FROM playlists p " +
                  "JOIN users u ON p.userId = u.userId " +
                  "WHERE p.userId = @userId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            
            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    playlists.Add(new Playlist
                    {
                        PlaylistId = reader.GetInt32("playlistId"),
                        PlaylistName = reader.GetString("playlistName"),
                        UserId = reader.GetInt32("userId"),
                        CreatedAt = reader.GetDateTime("createdAt"),
                        Username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString("username")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return playlists;
        }

        public bool AddSongToUserPlaylist(int playlistId, int songId)
        {
            using var conn = new MySqlConnection(connectionString);
            string query = "INSERT INTO playlistSongs (playlistId, songId) VALUES (@playlistId, @songId)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@playlistId", playlistId);
            cmd.Parameters.AddWithValue("@songId", songId);
            
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool DeleteUserPlaylistSong(int playlistId, int songId)
        {
            using var conn = new MySqlConnection(connectionString);
            string query = "DELETE FROM playlistSongs WHERE playlistId = @playlistId AND songId = @songId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@playlistId", playlistId);
            cmd.Parameters.AddWithValue("@songId", songId);
            
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool CreateUserPlaylist(string name, int userId)
        {
            using var conn = new MySqlConnection(connectionString);
            string query = "INSERT INTO playlists (playlistName, userId) VALUES (@name, @userId)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@userId", userId);
            
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public bool DeletePlaylist(int playlistId)
        {
            using var conn = new MySqlConnection(connectionString);
            string query = "DELETE FROM playlists WHERE playlistId = @playlistId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@playlistId", playlistId);
    
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery(); // Số dòng bị ảnh hưởng

                return rowsAffected > 0; // Trả về true nếu có ít nhất 1 dòng bị xóa
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; // Trả về false nếu có lỗi xảy ra
            }
        }

        public List<Playlist> SearchUserPlaylists(int userId, string playlistName)
        {
            List<Playlist> matchedPlaylists = new List<Playlist>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT p.*, u.username FROM playlists p " +
                      "JOIN users u ON p.userId = u.userId " +
                      "WHERE p.userId = @userId AND p.playlistName LIKE @playlistName";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@playlistName", $"%{playlistName}%");
                    
                    try
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                matchedPlaylists.Add(new Playlist
                                {
                                    PlaylistId = reader.GetInt32("playlistId"),
                                    PlaylistName = reader.GetString("playlistName"),
                                    UserId = reader.GetInt32("userId"),
                                    CreatedAt = reader.GetDateTime("createdAt"),
                                    Username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString("username")
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error searching playlists: {ex.Message}");
                    }
                }
            }
            return matchedPlaylists;
        }

        public List<Playlist> AdminSearchPlaylists(string playlistName)
        {
            List<Playlist> matchedPlaylists = new List<Playlist>();
            using var conn = new MySqlConnection(connectionString);
            string query = "SELECT p.*, u.username FROM playlists p " +
                        "JOIN users u ON p.userId = u.userId " +
                        "WHERE p.playlistName LIKE @playlistName " +
                        "ORDER BY p.createdAt DESC";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@playlistName", $"%{playlistName}%");
            
            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    matchedPlaylists.Add(new Playlist
                    {
                        PlaylistId = reader.GetInt32("playlistId"),
                        PlaylistName = reader.GetString("playlistName"),
                        UserId = reader.GetInt32("userId"),
                        CreatedAt = reader.GetDateTime("createdAt"),
                        Username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString("username")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching playlists: {ex.Message}");
            }
            
            return matchedPlaylists;
        }

        public List<Playlist> GetAllPlaylists()
        {
            List<Playlist> allPlaylists = new List<Playlist>();
            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT p.*, u.username FROM playlists p " +
                            "JOIN users u ON p.userId = u.userId " +
                            "ORDER BY p.createdAt DESC";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allPlaylists.Add(new Playlist
                                {
                                    PlaylistId = reader.GetInt32("playlistId"),
                                    PlaylistName = reader.GetString("playlistName"),
                                    UserId = reader.GetInt32("userId"),
                                    CreatedAt = reader.GetDateTime("createdAt"),
                                    Username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString("username")
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving all playlists: {ex.Message}");
                    }
                }
            }
            return allPlaylists;
        }
    }
}