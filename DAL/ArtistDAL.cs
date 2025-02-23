using System;
using System.Data;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class ArtistDAL
    {
        private readonly string connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=Duong7198!;";

        public void AddArtist(string artistName, DateTime birthDate, string topSong)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO artists (artistName, birthDate, topSong) VALUES (@artistName, @birthDate, @topSong)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@artistName", artistName);
                    cmd.Parameters.AddWithValue("@birthDate", birthDate);
                    cmd.Parameters.AddWithValue("@topSong", topSong);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Artist ? GetArtistById(int artistId)
        {
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand("SELECT * FROM artists WHERE artistId = @artistId", connection);

            command.Parameters.AddWithValue("@artistId", artistId);

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Artist
                    {
                        ArtistId = reader.GetInt32("artistId"),
                        ArtistName = reader.GetString("artistName"),
                        TopSong = reader.GetString("topSong"),
                        BirthDate = reader.IsDBNull("birthDate") ? null : reader.GetDateTime("birthDate")
                    };
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Database error: {ex.Message}");
            }
            return null;
        }

        public bool DeleteArtistById(int artistId)
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
                    System.Console.WriteLine("✅ Artist deleted successfully!");
                    return true;
                }
                else
                {
                    System.Console.WriteLine("❌ Failed to delete artist.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Database error: {ex.Message}");
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
                    BirthDate = reader.IsDBNull("birthDate") ? null : reader.GetDateTime("birthDate")
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
        return artists;
        }
    }
}