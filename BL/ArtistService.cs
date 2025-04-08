using System;
using DAL;
using Persistence;

namespace BL
{
    public class ArtistService
    {
        private readonly ArtistDAL artistDAL = new ArtistDAL();

        public Artist? GetArtistByUserId(int userId)
        {
            return artistDAL.GetArtistByUserId(userId);
        }

        public bool IsArtistRegistered(int userId)
        {
            return artistDAL.IsArtistRegistered(userId);
        }

        public bool RegisterArtist(string name, DateTime birthDate, string topSong, int userId)
        {
            if (IsArtistRegistered(userId))
            {
                return false;
            }

            artistDAL.AddArtist(name, birthDate, topSong, userId);
            return true;
        }

        public Artist? GetArtistByName(string artistName)
        {
            if (string.IsNullOrWhiteSpace(artistName))
                return null;
                
            return artistDAL.GetArtistByName(artistName);
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