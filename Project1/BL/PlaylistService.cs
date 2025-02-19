using DAL;
using Persistence.Models;

namespace BL
{
    public class PlaylistService
    {
        private readonly PlaylistDAL _playlistDAL = new();
        private readonly SongService _songService = new();

        public List<Playlist> GetUserPlaylists(int userId)
        {
            return _playlistDAL.GetUserPlaylists(userId);
        }

        public void AddSongToPlaylist(int playlistId, int songId)
        {
            _playlistDAL.AddSongToPlaylist(playlistId, songId);
        }

        public void RemoveSongFromPlaylist(int playlistId, int songId)
        {
            _playlistDAL.RemoveSongFromPlaylist(playlistId, songId);
        }

        public void CreatePlaylist(string name, int userId)
        {
            _playlistDAL.CreatePlaylist(name, userId);
        }
    }
}