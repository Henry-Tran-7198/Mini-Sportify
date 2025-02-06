using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class ArtistDAL
    {
        private readonly string connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=iLoveNOTP69;";

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

        
    }
}