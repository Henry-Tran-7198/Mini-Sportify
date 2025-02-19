using System;
using DAL;
using Persistence;
using Persistence.Models;

namespace BL
{
    public class SongService
    {
        private readonly SongDAL songDAL = new SongDAL();
        private readonly ArtistDAL artistDAL = new ArtistDAL();

        public bool UploadSong(string title, string artistName, string album, string genre, DateTime releaseDate)
        {
            // Get artist by name
            var artist = artistDAL.GetArtistByName(artistName);
            if (artist == null)
                return false;

            // Upload song with found artist ID
            songDAL.AddSong(title, artist.ArtistId, album, genre, releaseDate);
            return true;
        }

        public List<Song> GetSongsByArtist(int artistId)
        {
            return songDAL.GetSongsByArtist(artistId);
        }

        public bool DeleteSong(int songId, int artistId)
        {
            return songDAL.DeleteSong(songId, artistId);
        }
        public List<Song> GetAllSongs()
        {
            return songDAL.GetAllSongs();
        }

        public List<Song> GetPlaylistSongs(int playlistId)
        {
            return songDAL.GetPlaylistSongs(playlistId);
        }
    }
}