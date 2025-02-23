using System;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class PlaylistDAL
    {
        private readonly string connectionString = "Server=localhost;Database=SpotifyDB;Uid=root;Pwd=Duong7198!;";

        // Thêm playlist mới vào bảng `playlists` và trả về playlistId
        public int AddPlaylist(string playlistName)
        {
            int playlistId = 0;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO playlists (playlistName) VALUES (@playlistName); SELECT LAST_INSERT_ID();";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@playlistName", playlistName);
                    playlistId = Convert.ToInt32(cmd.ExecuteScalar()); // Lấy ID của playlist vừa tạo
                }
            }
            return playlistId;
        }

        // Liên kết bài hát với playlist trong bảng `playlist_songs`
        public void AddSongToPlaylist(int playlistId, int songId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO playlist_songs (playlistId, songId) VALUES (@playlistId, @songId)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@playlistId", playlistId);
                    cmd.Parameters.AddWithValue("@songId", songId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Playlist ? GetPlaylistById(int playlistId)
        {
            var playlist = new Playlist();
            var songs = new List<Song>();
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand("SELECT playlists.playlistId, playlists.playlistName, songs.title FROM playlists JOIN playlist_songs ON playlists.playlistId = playlist_songs.playlistId JOIN songs ON playlist_songs.songId = songs.songId WHERE playlists.playlistId = @playlistId", connection);
            command.Parameters.AddWithValue("@playlistId", playlistId);

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (playlist.PlaylistId == 0)
                    {
                        playlist.PlaylistId = reader.GetInt32("playlistId");
                        playlist.PlaylistName = reader.GetString("playlistName");
                    }
                    // ✅ Tạo đối tượng Song và thêm vào danh sách
                    var song = new Song 
                    {
                        Title = reader.GetString("title")
                        // Thêm các property khác nếu cần
                    };
                    songs.Add(song);
                }
                playlist.Songs = songs;
                return playlist;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }

        public bool DeletePlaylistById(int playlistId)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
    
            //Bắt đầu transaction để đảm bảo toàn vẹn dữ liệu
            using var transaction = connection.BeginTransaction();

            try
            {
                //Bước 1: Lấy danh sách songId từ playlist_songs
                var songIds = new List<int>();
                using (var cmdGetSongs = new MySqlCommand(
                    "SELECT songId FROM playlist_songs WHERE playlistId = @playlistId", connection, transaction
                ))
                {
                    cmdGetSongs.Parameters.AddWithValue("@playlistId", playlistId);
                    using var reader = cmdGetSongs.ExecuteReader();
                    while (reader.Read())
                    {
                        songIds.Add(reader.GetInt32(0)); 
                    }
                }

                //Bước 2: Xoá liên kết trong playlist_songs
                using (var cmdDeleteLinks = new MySqlCommand(
                    "DELETE FROM playlist_songs WHERE playlistId = @playlistId", connection, transaction
                ))
                {
                    cmdDeleteLinks.Parameters.AddWithValue("@playlistId", playlistId);
                    cmdDeleteLinks.ExecuteNonQuery();
                }

                //Bước 3: Xoá bài hát trong songs (nếu không còn liên kết)
                if (songIds.Count > 0)
                {
                    var query = @"DELETE FROM songs WHERE songId IN ({0})
                                 AND NOT EXISTS (
                                 SELECT 1 FROM playlist_songs ps
                                 WHERE ps.songId = songs.songId
                                )";

                    var parameters = new List<string>();
                    for (int i = 0; i < songIds.Count; i++)
                    {
                        parameters.Add($"@songId{i}");
                    }

                    query = string.Format(query, string.Join(",", parameters));

                    using var cmdDeleteSongs = new MySqlCommand(query, connection, transaction);
            
                    for (int i = 0; i < songIds.Count; i++)
                    {
                        cmdDeleteSongs.Parameters.AddWithValue($"@songId{i}", songIds[i]);
                    }
                    cmdDeleteSongs.ExecuteNonQuery();
                }
                //Commit transaction nếu thành công
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //Rollback nếu có lỗi
                transaction.Rollback();
                System.Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public List<Playlist> GetAllPlaylists()
        {
            List<Playlist> playlists = new List<Playlist>();
            using var connection = new MySqlConnection(connectionString);
            using var command = new MySqlCommand(@"
                SELECT p.playlistId, p.playlistName, s.songId, s.title
                FROM playlists p
                JOIN playlist_songs ps ON p.playlistId = ps.playlistId
                JOIN songs s ON ps.songId = s.songId", connection);            
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();

                var playlistDict = new Dictionary<int, Playlist>();

                while (reader.Read())
                {
                    int playlistId = reader.GetInt32("playlistId");

                    if(!playlistDict.ContainsKey(playlistId))
                    {
                        playlistDict[playlistId] = new Playlist
                        {
                            PlaylistId = playlistId,
                            PlaylistName = reader.GetString("playlistName"),
                            Songs = new List<Song>() //Khởi tạo danh sách bài hát
                        };
                    }
                    // ✅ Tạo đối tượng Song và thêm vào danh sách
                    var song = new Song 
                    {   
                        SongId = reader.GetInt32("songId"),
                        Title = reader.GetString("title")
                    };
                    playlistDict[playlistId].Songs.Add(song);
                }
                //Chuyển từ dictionary sang list
                playlists = playlistDict.Values.ToList();
                return playlists;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return playlists;
            }
        }
    }
}