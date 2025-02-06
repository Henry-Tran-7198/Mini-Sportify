namespace Persistence;

public class Song
{
    public int SongId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ArtistId { get; set; }
    public string Album { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; }

    public Song() { }

    public Song(string title, int artistId, string album, string genre, DateTime releaseDate)
    {
        Title = title;
        ArtistId = artistId;
        Album = album;
        Genre = genre;
        ReleaseDate = releaseDate;
    }

    public override string ToString()
    {
        return $"║ {SongId,-9} ║ {Title,-20} ║ {ArtistId,-9} ║ {Album,-30} ║ {Genre,-15} ║ {ReleaseDate?.ToString("yyyy-MM-dd") ?? "N/A",-10} ║";
    }
}
