using System;
using System.Collections.Generic;
using DAL;
using Persistence; // 🔥 Thêm dòng này để nhận diện Song

namespace BL
{
    public class SongService
    {
        private SongDAL songDAL = new SongDAL();

        public void UploadSong(string title, int artistId, string album, string genre, DateTime releaseDate)
        {
            songDAL.AddSong(title, artistId, album, genre, releaseDate);
        }

        public List<Song> GetSongByArtistId(int artistId)
        {
            return songDAL.GetSongsByArtistId(artistId); 
        }

        public Song? SearchSong(int songId)
        {
            if (songId <= 0)
            {
                Console.WriteLine("❌ Invalid ID. Please enter a valid number.");
                return null;
            }

            var song = songDAL.GetSongById(songId); 

            if (song == null)
            {
                Console.WriteLine("❌ Song not found.");
            }
            return song;
        }

        public bool DeleteSong(int songId)
        {
            SongDAL songDAL = new SongDAL(); //Tạo 1 đối tượng SongDAL để làm việc với database
            bool isDeleted = songDAL.DeleteSongById(songId); //Gọi phương thức DeleteSongById trong songId

            if (isDeleted)
            {
                return true;
            }
            else
            {
                System.Console.WriteLine("❌ User not found!");
                return false;
            }
        }

        public List<Song> GetAllSongs()
        {
            return songDAL.GetAllSongs();  
        }
    }
}
