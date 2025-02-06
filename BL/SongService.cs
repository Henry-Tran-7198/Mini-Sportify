using System;
using DAL;

namespace BL
{
    public class SongService
    {
        private SongDAL songDAL = new SongDAL();

        public void UploadSong(string title, int artistId, string album, string genre, DateTime releaseDate)
        {
            songDAL.AddSong(title, artistId, album, genre, releaseDate);
        }
    }
}
