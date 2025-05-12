using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class ArtistDAL
    {
        private readonly string connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=iLoveNOTP69;";

        public Artist? GetArtistByUserId(int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM artists WHERE userId = @userId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Artist
                            {
                                ArtistId = reader.GetInt32("artistId"),
                                ArtistName = reader.GetString("artistName"),
                                BirthDate = reader.GetDateTime("birthDate"),
                                TopSong = reader.GetString("topSong"),
                                UserId = reader.GetInt32("userId")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool IsArtistRegistered(int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM artists WHERE userId = @userId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public List<Artist> SearchArtists(string keyword)
        {
            List<Artist> matchedArtists = new List<Artist>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM artists WHERE artistName LIKE @keyword";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())  // Dùng while thay vì if để lấy tất cả kết quả
                        {
                            matchedArtists.Add(new Artist
                            {
                                ArtistId = reader.GetInt32("artistId"),
                                ArtistName = reader.GetString("artistName"),
                                BirthDate = reader["birthDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["birthDate"]),
                                TopSong = reader.GetString("topSong"),
                                UserId = reader.GetInt32("userId")
                            });
                        }
                    }
                }
            }
            return matchedArtists;
        }

        public bool DeleteArtist(int artistId)
        {
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand("DELETE FROM artists WHERE artistId = @artistId", connection);

            command.Parameters.AddWithValue("@artistId", artistId);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Artist deleted successfully!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to delete artist.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        public List<Artist> GetAllArtists()
        {
            List<Artist> artists = new List<Artist>();
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand("SELECT * FROM artists", connection);

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    artists.Add(new Artist
                    {
                        ArtistId = reader.GetInt32("artistId"),
                        ArtistName = reader.GetString("artistName"),
                        TopSong = reader.GetString("topSong"),
                        BirthDate = reader["birthDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["birthDate"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            return artists;
        }

        public bool RegisterArtist(Artist artist)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // First check if artist already exists for this user
                    if (IsArtistRegistered(artist.UserId))
                    {
                        return false;
                    }
                    
                    string query = @"INSERT INTO artists (artistName, birthDate, topSong, userId) 
                                VALUES (@artistName, @birthDate, @topSong, @userId)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@artistName", artist.ArtistName);
                        cmd.Parameters.AddWithValue("@birthDate", artist.BirthDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@topSong", artist.TopSong);
                        cmd.Parameters.AddWithValue("@userId", artist.UserId);
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error during artist registration: {ex.Message}");
                return false;
            }
        }
    }
}