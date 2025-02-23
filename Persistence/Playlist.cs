namespace Persistence;

public class Playlist
{
    public int PlaylistId { get; set; }
    public string PlaylistName { get; set; } = string.Empty;
    public List<Song> Songs { get; set; } = new List<Song>(); 

    //Constructor mặc định
    public Playlist() 
    {
        Songs = new List<Song>();
    }

    //Constructor có tham số
    public Playlist(string playlistName)
    {
        PlaylistName = playlistName;
        Songs = new List<Song>();
    }
}