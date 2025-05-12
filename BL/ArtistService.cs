using DAL;
using Persistence;

namespace BL
{
    public class ArtistService
    {
        private readonly ArtistDAL artistDAL;
        private readonly SongDAL songDAL;

        public ArtistService()
        {
            artistDAL = new ArtistDAL();
            songDAL = new SongDAL();
        }

        // Đăng ký làm nghệ sĩ
        public bool RegisterArtist(Artist artist)
        {
            if (artist == null || string.IsNullOrWhiteSpace(artist.ArtistName))
            {
                return false;
            }
            
            // Kiểm tra người dùng đã là nghệ sĩ chưa
            if (artistDAL.IsArtistRegistered(artist.UserId))
            {
                return false;
            }
            
            return artistDAL.RegisterArtist(artist);
        }

        // Kiểm tra người dùng đã đăng ký làm nghệ sĩ chưa
        public bool IsArtistRegistered(int userId)
        {
            return artistDAL.IsArtistRegistered(userId);
        }

        // Lấy thông tin nghệ sĩ theo userId
        public Artist GetArtistByUserId(int userId)
        {
            return artistDAL.GetArtistByUserId(userId);
        }

        // Lấy tất cả nghệ sĩ
        public List<Artist> GetAllArtists()
        {
            return artistDAL.GetAllArtists();
        }

        // Tìm kiếm nghệ sĩ
        public List<Artist> AdminSearchArtists(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Artist>();
            }
            
            return artistDAL.SearchArtists(keyword);
        }

        // Thêm bài hát (cho nghệ sĩ)
        public bool AddArtistSong(string title, int userId, string album, string genre, DateTime? releaseDate)
        {
            var artist = artistDAL.GetArtistByUserId(userId);
            if (artist == null)
            {
                return false;
            }
            
            return songDAL.AddArtistSong(title, artist.ArtistId, album, genre, releaseDate);
        }

        // Xóa nghệ sĩ (cho Admin)
        public bool AdminDeleteArtist(int artistId)
        {
            return artistDAL.DeleteArtist(artistId);
        }
    }
}