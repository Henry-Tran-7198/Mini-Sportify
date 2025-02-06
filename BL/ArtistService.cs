using System;
using DAL;

namespace BL
{
    public class ArtistService
    {
        private ArtistDAL artistDAL = new ArtistDAL();

        public void RegisterArtist(string name, DateTime birthDate, string topSong)
        {
            artistDAL.AddArtist(name, birthDate, topSong);
        }
    }
}
