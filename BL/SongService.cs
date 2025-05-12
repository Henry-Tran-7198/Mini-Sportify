using DAL;
using Persistence;

namespace BL
{
    public class SongService
    {
        private readonly SongDAL _songDAL = new();
        private readonly ArtistDAL _artistDAL = new();

        /// Thêm bài hát mới với artistId
        public bool AddArtistSong(string title, int artistId, string album, string genre, DateTime? releaseDate)
        {
            return _songDAL.AddArtistSong(title, artistId, album, genre, releaseDate);
        }

        /// Lấy danh sách bài hát của nghệ sĩ
        public List<Song> GetArtistSongs(int artistId)
        {
            return _songDAL.GetArtistSongs(artistId);
        }

        /// Xóa bài hát với phân quyền
        public bool DeleteArtistSong(int songId, int userId, string role)
        {
            // Admin có thể xóa bất kỳ bài hát nào
            if (role.Contains("Admin"))
            {
                return _songDAL.AdminDeleteArtistSong(songId);
            }
            
            // Artist chỉ có thể xóa bài hát của mình
            if (role.Contains("Artist"))
            {
                var artist = _artistDAL.GetArtistByUserId(userId);
                if (artist == null)
                {
                    return false; // Người dùng không phải là nghệ sĩ
                }
                
                return _songDAL.DeleteArtistSong(songId, artist.ArtistId);
            }
            
            return false; // Người dùng không có quyền xóa bài hát
        }

        /// Lấy tất cả bài hát
        public List<Song> GetAllSongs()
        {
            return _songDAL.GetAllSongs();
        }

        /// Lấy bài hát trong playlist
        public List<Song> GetUserPlaylistSongs(int playlistId, int userId, string role)
        {
            // Admin có quyền truy cập mọi playlist
            if (role.ToLower().Contains("admin"))
            {
                return _songDAL.GetUserPlaylistSongs(playlistId);
            }
            
            // Người dùng thường - cần kiểm tra quyền sở hữu
            var playlistDAL = new PlaylistDAL();
            var userPlaylists = playlistDAL.GetUserPlaylists(userId);
            bool isOwner = userPlaylists.Any(p => p.PlaylistId == playlistId);
            
            if (isOwner)
            {
                // Người dùng sở hữu playlist này
                return _songDAL.GetUserPlaylistSongs(playlistId);
            }
            else 
            {
                // TODO: Nếu có thêm trường isPublic trong Playlist, có thể thêm kiểm tra playlist công khai ở đây
                // Hiện tại, giả định người dùng chỉ xem được playlist của mình
                return new List<Song>();
            
            }
        }

        /// Tìm kiếm bài hát
        public List<Song> SearchSongs(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Song>();
            }
            
            return _songDAL.SearchSongs(keyword);
        }

        /// Tìm kiếm bài hát trong playlist
        public List<Song> SearchUserPlaylistSongs(int playlistId, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Song>();
            }
            
            return _songDAL.SearchUserPlaylistSongs(playlistId, keyword);
        }
    }
}