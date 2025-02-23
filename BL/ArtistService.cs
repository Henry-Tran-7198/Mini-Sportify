using System;
using DAL;
using Persistence;

namespace BL
{
    public class ArtistService
    {
        private ArtistDAL artistDAL = new ArtistDAL(); // Không có dấu "_"

        public void RegisterArtist(string name, DateTime birthDate, string topSong)
        {
            artistDAL.AddArtist(name, birthDate, topSong);
        }

        public Artist? SearchArtist(int artistId)
        {
            if (artistId <= 0)
            {
                Console.WriteLine("❌ Invalid ID. Please enter a valid number.");
                return null;
            }

            var artist = artistDAL.GetArtistById(artistId); // 🔹 Gọi DAL để lấy thông tin nghệ sĩ

            if (artist == null)
            {
                Console.WriteLine("❌ Artist not found.");
            }

            return artist;
        }

        public bool DeleteArtist(int artistId)
        {
            ArtistDAL artistDAL = new ArtistDAL(); //Tạo 1 đối tượng ArtistDAL để làm việc với database
            bool isDeleted = artistDAL.DeleteArtistById(artistId); //Gọi phương thức DeleteUserById trong UserId

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

        public List<Artist> GetAllArtists()
        {
            return artistDAL.GetAllArtists();  
        }
    }
}
