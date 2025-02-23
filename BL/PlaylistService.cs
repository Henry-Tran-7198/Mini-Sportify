using System;
using System.Collections.Generic;
using DAL;
using Persistence;
using MySql.Data.MySqlClient;

namespace BL
{
    public class PlaylistService
    {
        private readonly PlaylistDAL playlistDAL = new PlaylistDAL();
        private readonly SongDAL songDAL = new SongDAL();

        // Tạo playlist mới và thêm các bài hát
        public bool CreatePlaylist(string playlistName, List<Song> songs)
        {
            try
            {
                // Thêm playlist vào bảng `playlists`
                int playlistId = playlistDAL.AddPlaylist(playlistName);

                // Thêm từng bài hát vào bảng `songs` và liên kết với playlist
                foreach (var song in songs)
                {
                    // Thêm bài hát vào bảng `songs`
                    songDAL.AddSong(song.Title, song.ArtistId, song.Album, song.Genre, song.ReleaseDate);

                    // Lấy songId của bài hát vừa thêm (giả sử songId là tự tăng)
                    int songId = GetLastInsertedSongId();

                    // Liên kết bài hát với playlist trong bảng `playlist_songs`
                    playlistDAL.AddSongToPlaylist(playlistId, songId);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating playlist: {ex.Message}");
                return false;
            }
        }

        // Lấy songId của bài hát vừa thêm vào bảng `songs`
        private int GetLastInsertedSongId()
        {
            int songId = 0;
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=SpotifyDB;Uid=root;Pwd=Duong7198!;"))
            {
                conn.Open();
                string query = "SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    songId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return songId;
        }

        public Playlist? SearchPlaylist(int playlistId)
        {
            if (playlistId <= 0)
            {
                Console.WriteLine("❌ Invalid ID. Please enter a valid number.");
                return null;
            }

            var playlist = playlistDAL.GetPlaylistById(playlistId); // 🔹 Gọi DAL để lấy thông tin nghệ sĩ

            if (playlist == null)
            {
                Console.WriteLine("❌ Playlist not found.");
            }

            if (playlist.Songs == null || playlist.Songs.Count == 0)
            {
                Console.WriteLine("Playlist is empty.");
            }

            return playlist;
        }

        public bool DeletePlaylist(int playlistId)
        {
            PlaylistDAL playlistDAL = new PlaylistDAL(); //Tạo 1 đối tượng PlaylistDAL để làm việc với database
            bool isDeleted = playlistDAL.DeletePlaylistById(playlistId); //Gọi phương thức DeletePlaylistById trong playlistId

            if (isDeleted)
            {
                return true;
            }
            else
            {
                System.Console.WriteLine("❌ Playlist not found!");
                return false;
            }
        }

        public List<Playlist> GetAllPlaylists()
        {
            return playlistDAL.GetAllPlaylists(); 
        }
    }
}