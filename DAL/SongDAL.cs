using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class SongDAL
    {
        private readonly string connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=iLoveNOTP69;";

        public bool AddArtistSong(string title, int artistId, string album, string genre, DateTime? releaseDate)
        {
            using var conn = new MySqlConnection(connectionString);
            string query = "INSERT INTO songs (title, artistId, album, genre, releaseDate) VALUES (@title, @artistId, @album, @genre, @releaseDate)";
            using var cmd = new MySqlCommand(query, conn);
            
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@artistId", artistId);
            cmd.Parameters.AddWithValue("@album", album);
            cmd.Parameters.AddWithValue("@genre", genre);
            cmd.Parameters.AddWithValue("@releaseDate", releaseDate ?? (object)DBNull.Value);
            
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding song: {ex.Message}");
                return false;
            }
        }

        public bool DeleteArtistSong(int songId, int artistId)
        {
            using var connection = new MySqlConnection(connectionString);
            string query = "DELETE FROM songs WHERE songId = @songId AND artistId = @artistId";
            using var command = new MySqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@songId", songId);
            command.Parameters.AddWithValue("@artistId", artistId);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting song: {ex.Message}");
                return false;
            }
        }

        public bool AdminDeleteArtistSong(int songId)
        {
            using var connection = new MySqlConnection(connectionString);
            string query = "DELETE FROM songs WHERE songId = @songId";
            using var command = new MySqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@songId", songId);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting song: {ex.Message}");
                return false;
            }
        }

        public List<Song> GetArtistSongs(int artistId)
        {
            List<Song> songs = new List<Song>();
            using var connection = new MySqlConnection(connectionString);
            string query = @"SELECT s.*, a.artistName 
                            FROM songs s 
                            JOIN artists a ON s.artistId = a.artistId 
                            WHERE s.artistId = @artistId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@artistId", artistId);

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    songs.Add(new Song
                    {
                        SongId = reader.GetInt32("songId"),
                        Title = reader.GetString("title"),
                        ArtistId = reader.GetInt32("artistId"),
                        ArtistName = reader.IsDBNull(reader.GetOrdinal("artistName")) ? "Unknown" : reader.GetString("artistName"),
                        Album = reader.GetString("album"),
                        Genre = reader.GetString("genre"),
                        ReleaseDate = reader.GetDateTime("releaseDate")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting songs: {ex.Message}");
            }
            return songs;
        }

        public List<Song> GetAllSongs()
        {
            List<Song> songs = new List<Song>();
            using var connection = new MySqlConnection(connectionString);
            string query = @"SELECT s.*, a.artistName 
                            FROM songs s 
                            LEFT JOIN artists a ON s.artistId = a.artistId
                            ORDER BY s.songId DESC";

            using var command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    songs.Add(new Song
                    {
                        SongId = reader.GetInt32("songId"),
                        Title = reader.GetString("title"),
                        ArtistId = reader.GetInt32("artistId"),
                        ArtistName = reader.IsDBNull(reader.GetOrdinal("artistName")) ? "Unknown" : reader.GetString("artistName"),
                        Album = reader.GetString("album"),
                        Genre = reader.GetString("genre"),
                        ReleaseDate = reader.GetDateTime("releaseDate")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting songs: {ex.Message}");
            }
            return songs;
        }

        public List<Song> GetUserPlaylistSongs(int playlistId)
        {
            List<Song> songs = new List<Song>();
            using var connection = new MySqlConnection(connectionString);
            string query = @"SELECT s.*, a.artistName 
                            FROM songs s 
                            LEFT JOIN artists a ON s.artistId = a.artistId
                            JOIN playlistSongs ps ON s.songId = ps.songId
                            WHERE ps.playlistId = @playlistId
                            ORDER BY ps.createdAt DESC";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@playlistId", playlistId);

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    songs.Add(new Song
                    {
                        SongId = reader.GetInt32("songId"),
                        Title = reader.GetString("title"),
                        ArtistId = reader.GetInt32("artistId"),
                        ArtistName = reader.IsDBNull(reader.GetOrdinal("artistName")) ? "Unknown" : reader.GetString("artistName"),
                        Album = reader.GetString("album"),
                        Genre = reader.GetString("genre"),
                        ReleaseDate = reader.GetDateTime("releaseDate")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting playlist songs: {ex.Message}");
            }
            return songs;
        }

        public List<Song> SearchSongs(string keyword)
        {
            List<Song> matchedSongs = new List<Song>();
            using var conn = new MySqlConnection(connectionString);
            string query = @"SELECT s.*, a.artistName 
                            FROM songs s 
                            LEFT JOIN artists a ON s.artistId = a.artistId
                            WHERE s.title LIKE @keyword 
                            OR s.album LIKE @keyword 
                            OR a.artistName LIKE @keyword
                            ORDER BY s.title";
                            
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    matchedSongs.Add(new Song
                    {
                        SongId = reader.GetInt32("songId"),
                        Title = reader.GetString("title"),
                        ArtistId = reader.GetInt32("artistId"),
                        ArtistName = reader.IsDBNull(reader.GetOrdinal("artistName")) ? "Unknown" : reader.GetString("artistName"),
                        Album = reader.GetString("album"),
                        Genre = reader.GetString("genre"),
                        ReleaseDate = reader.GetDateTime("releaseDate")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching songs: {ex.Message}");
            }
            return matchedSongs;
        }

        public List<Song> SearchUserPlaylistSongs(int playlistId, string keyword)
        {
            List<Song> matchedSongs = new List<Song>();
            using var conn = new MySqlConnection(connectionString);
            string query = @"SELECT s.*, a.artistName 
                            FROM songs s 
                            LEFT JOIN artists a ON s.artistId = a.artistId
                            JOIN playlistSongs ps ON s.songId = ps.songId
                            WHERE ps.playlistId = @playlistId
                            AND (s.title LIKE @keyword 
                                OR s.album LIKE @keyword 
                                OR a.artistName LIKE @keyword)
                            ORDER BY s.title";
                            
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@playlistId", playlistId);
            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

            try
            {
                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    matchedSongs.Add(new Song
                    {
                        SongId = reader.GetInt32("songId"),
                        Title = reader.GetString("title"),
                        ArtistId = reader.GetInt32("artistId"),
                        ArtistName = reader.IsDBNull(reader.GetOrdinal("artistName")) ? "Unknown" : reader.GetString("artistName"),
                        Album = reader.GetString("album"),
                        Genre = reader.GetString("genre"),
                        ReleaseDate = reader.GetDateTime("releaseDate")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching playlist songs: {ex.Message}");
            }
            return matchedSongs;
        }
    }
}