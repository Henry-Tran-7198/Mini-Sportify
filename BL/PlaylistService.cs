using DAL;
using Persistence.Models;

namespace BL
{
    public class PlaylistService
    {
        private readonly PlaylistDAL _playlistDAL = new();

        /// Lấy danh sách playlist của người dùng
        public List<Playlist> GetUserPlaylists(int userId)
        {
            return _playlistDAL.GetUserPlaylists(userId);
        }

        /// Lấy tất cả playlist trong hệ thống (dành cho Admin)
        public List<Playlist> GetAllPlaylists()
        {
            return _playlistDAL.GetAllPlaylists();
        }

        /// Tìm kiếm playlist của một người dùng cụ thể
        public List<Playlist> SearchUserPlaylists(int userId, string playlistName)
        {
            if (string.IsNullOrWhiteSpace(playlistName))
            {
                return new List<Playlist>();
            }
            
            return _playlistDAL.SearchUserPlaylists(userId, playlistName);
        }

        /// Tìm kiếm tất cả playlist trong hệ thống (dành cho Admin)
        public List<Playlist> AdminSearchPlaylists(string playlistName)
        {
            if (string.IsNullOrWhiteSpace(playlistName))
            {
                return new List<Playlist>();
            }
            
            return _playlistDAL.AdminSearchPlaylists(playlistName);
        }

        /// Thêm bài hát vào playlist
        public bool AddSongToUserPlaylist(int playlistId, int songId, int userId)
        {
            // Kiểm tra xem playlist có thuộc về người dùng không
            var userPlaylists = _playlistDAL.GetUserPlaylists(userId);
            bool isOwner = userPlaylists.Any(p => p.PlaylistId == playlistId);
            
            if (!isOwner)
            {
                return false; // Không có quyền thêm bài hát vào playlist này
            }
            
            _playlistDAL.AddSongToUserPlaylist(playlistId, songId);
            return true;
        }

        /// Xóa bài hát khỏi playlist
        public bool DeleteUserPlaylistSong(int playlistId, int songId, int userId)
        {
            // Kiểm tra xem playlist có thuộc về người dùng không
            var userPlaylists = _playlistDAL.GetUserPlaylists(userId);
            bool isOwner = userPlaylists.Any(p => p.PlaylistId == playlistId);
            
            if (!isOwner)
            {
                return false; // Không có quyền xóa bài hát khỏi playlist này
            }
            
            _playlistDAL.DeleteUserPlaylistSong(playlistId, songId);
            return true;
        }

        /// Tạo playlist mới
        public bool CreateUserPlaylist(string name, int userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            
            _playlistDAL.CreateUserPlaylist(name, userId);
            return true;
        }

        /// Phương thức tổng quát cho việc xóa playlist, tự phân quyền dựa trên role
        public bool DeletePlaylist(int playlistId, int userId, string role)
        {
            // Admin có thể xóa bất kỳ playlist nào
            if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return _playlistDAL.DeletePlaylist(playlistId);
            }
            
            // Listener chỉ có thể xóa playlist của mình
            if (role.Equals("listener", StringComparison.OrdinalIgnoreCase))
            {
                var userPlaylists = _playlistDAL.GetUserPlaylists(userId);
                bool isOwner = userPlaylists.Any(p => p.PlaylistId == playlistId);
                
                if (!isOwner)
                {
                    return false; // Không có quyền xóa playlist này
                }
                
                return _playlistDAL.DeletePlaylist(playlistId);
            }
            
            return false; // Không có quyền xóa
        }
    }
}