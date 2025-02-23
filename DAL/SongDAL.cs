using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class SongDAL
    {
        private readonly string connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=Duong7198!;";

        public void AddSong(string title, int artistId, string album, string genre, DateTime? releaseDate)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO songs (title, artistId, album, genre, releaseDate) VALUES (@title, @artistId, @album, @genre, @releaseDate)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@artistId", artistId);
                    cmd.Parameters.AddWithValue("@album", album);
                    cmd.Parameters.AddWithValue("@genre", genre);
                    cmd.Parameters.AddWithValue("@releaseDate", releaseDate ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Song> GetSongsByArtistId(int artistId)
        {
            List<Song> songs = new List<Song>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM songs WHERE artistId = @artistId";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@artistId", artistId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Song song = new Song
                            {
                                SongId = reader.GetInt32("songId"),
                                Title = reader.GetString("title"),
                                ArtistId = reader.GetInt32("artistId"),
                                Album = reader.GetString("album"),
                                Genre = reader.GetString("genre"),
                                ReleaseDate = reader.IsDBNull("releaseDate") ? (DateTime?)null : reader.GetDateTime("releaseDate")
                            };
                            songs.Add(song);
                        }
                    }
                }
            }

            return songs;
        }

        public Song ? GetSongById(int songId)
        {
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand("SELECT * FROM songs WHERE songId = @songId", connection);

            command.Parameters.AddWithValue("@songId", songId);

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Song
                    {
                        SongId = reader.GetInt32("songId"),
                        Title = reader.GetString("title"),
                        Album = reader.GetString("album"),
                        Genre = reader.GetString("genre")
                    };
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Database error: {ex.Message}");
            }
            return null;
        }

        public bool DeleteSongById(int songId)
        {
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand("DELETE FROM songs WHERE songId = @songId", connection);

            command.Parameters.AddWithValue("@songId", songId);
            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    System.Console.WriteLine("✅ Song deleted successfully!");
                    return true;
                }
                else
                {
                    System.Console.WriteLine("❌ Failed to delete song.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        public List<Song> GetAllSongs()
        {
            List<Song> songs = new List<Song>();
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand("SELECT * FROM songs", connection);

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
                        Album = reader.GetString("album"),
                        Genre = reader.GetString("genre")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            return songs;
        }
    }
}
