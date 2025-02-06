using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class SongDAL
    {
        private readonly string connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=iLoveNOTP69;";

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
    }
}