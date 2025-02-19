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
            string query = "SELECT * FROM playlists WHERE userId = @userId";
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
                        CreatedAt = reader.GetDateTime("createdAt")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return playlists;
        }

        public void AddSongToPlaylist(int playlistId, int songId)
        {
            using var conn = new MySqlConnection(connectionString);
            string query = "INSERT INTO playlistSongs (playlistId, songId) VALUES (@playlistId, @songId)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@playlistId", playlistId);
            cmd.Parameters.AddWithValue("@songId", songId);
            
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void RemoveSongFromPlaylist(int playlistId, int songId)
        {
            using var conn = new MySqlConnection(connectionString);
            string query = "DELETE FROM playlistSongs WHERE playlistId = @playlistId AND songId = @songId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@playlistId", playlistId);
            cmd.Parameters.AddWithValue("@songId", songId);
            
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void CreatePlaylist(string name, int userId)
        {
            using var conn = new MySqlConnection(connectionString);
            string query = "INSERT INTO playlists (playlistName, userId) VALUES (@name, @userId)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@userId", userId);
            
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}