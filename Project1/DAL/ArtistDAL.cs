using System;
using System.Data;
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
        public void AddArtist(string artistName, DateTime birthDate, string topSong, int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO artists (artistName, birthDate, topSong, userId) VALUES (@artistName, @birthDate, @topSong, @userId)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@artistName", artistName);
                    cmd.Parameters.AddWithValue("@birthDate", birthDate);
                    cmd.Parameters.AddWithValue("@topSong", topSong);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Artist? GetArtistByName(string artistName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM artists WHERE artistName LIKE @name";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", $"%{artistName}%");
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Artist
                            {
                                ArtistId = reader.GetInt32("artistId"),
                                ArtistName = reader.GetString("artistName"),
                                BirthDate = reader.GetDateTime("birthDate"),
                                TopSong = reader.GetString("topSong")
                            };
                        }
                    }
                }
            }
            return null;
        }

        
    }
}