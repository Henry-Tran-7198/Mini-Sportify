namespace Persistence;

public class Artist
{
    public int ArtistId { get; set; }
    public string ArtistName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string TopSong { get; set; } = string.Empty;
    
    public Artist() { }

    public Artist(string artistName, DateTime? birthDate, string topSong)
    {
        ArtistName = artistName;
        BirthDate = birthDate;
        TopSong = topSong;
    }

    public override string ToString()
    {
        return $"║ {ArtistId,-9} ║ {ArtistName,-20} ║ {BirthDate?.ToString("yyyy-MM-dd") ?? "N/A",-10} ║ {TopSong,-25} ║";
    }
}
