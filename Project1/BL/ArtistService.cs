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

    

    }

}