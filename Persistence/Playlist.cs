namespace Persistence.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; } = string.Empty;
        public int UserId { get; set;}
        public DateTime CreatedAt { get; set; }
    }
}