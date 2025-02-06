CREATE DATABASE SpotifyDB;

USE SpotifyDB;

//Create table users
CREATE TABLE users(
    userId INT PRIMARY KEY AUTO_INCREMENT,
    userName VARCHAR(50) NOT NULL,
    userEmail VARCHAR(50) NOT NULL,
    userPassword VARCHAR(50) NOT NULL,
    roles VARCHAR(50) NOT NULL
);

//Create table artists
CREATE TABLE artists(
    artistId INT PRIMARY KEY AUTO_INCREMENT,
    artistName VARCHAR(100) NOT NULL,
    birthDate DATE,
    topSong VARCHAR(100)
);

//Create table songs
CREATE TABLE songs(
    songId INT PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(100) NOT NULL,
    artistId INT,
    album VARCHAR(100),
    genre VARCHAR(50), 
    releaseDate DATE,
    FOREIGN KEY (artistId) REFERENCES artists(artistId) ON DELETE SET NULL
);

//Create table playlists
CREATE TABLE playlists(
    playlistId INT PRIMARY KEY AUTO_INCREMENT,
    playlistName VARCHAR(100) NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

//Create table playlist_songs: Connection between songs and playlists
CREATE TABLE playlist_songs(
    playlistId INT NOT NULL,
    songId INT NOT NULL,
    PRIMARY KEY(playlistId, songId),
    FOREIGN KEY(playlistId) REFERENCES playlists(playlistId) ON DELETE CASCADE,
    FOREIGN KEY(songId) REFERENCES songs(songId) ON DELETE CASCADE
);

//Create table liked_songs: User Music Library
CREATE TABLE liked_songs(
    likeId INT PRIMARY KEY AUTO_INCREMENT,
    userId INT NOT NULL,
    songId INT NOT NULL,
    likedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (userId) REFERENCES users(userId) ON DELETE CASCADE,
    FOREIGN KEY (songId) REFERENCES songs(songId) ON DELETE CASCADE
);


CREATE TABLE myPlaylist(
    myPlaylistId INT PRIMARY KEY AUTO_INCREMENT,
    userId INT NOT NULL,
    playlistName VARCHAR(100) NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (userId) REFERENCES users(userId) ON DELETE CASCADE
);


CREATE TABLE myPlaylist_songs(
    myPlaylistId INT NOT NULL,
    songId INT NOT NULL,
    PRIMARY KEY (myPlaylistId, songId),
    FOREIGN KEY (myPlaylistId) REFERENCES myPlaylist(myPlaylistId) ON DELETE CASCADE,
    FOREIGN KEY (songId) REFERENCES songs(songId) ON DELETE CASCADE
);

INSERT INTO artists (artistId, artistName, birthDate, topSong) VALUES
(1, 'Ed Sheeran', '1991-02-17', 'Shape of You'),
(2, 'The Weeknd', '1990-02-16', 'Blinding Lights'),
(3, 'Adele', '1988-05-05', 'Someone Like You'),
(4, 'Mark Ronson', '1975-09-04', 'Uptown Funk'),
(5, 'Queen', '1970-01-01', 'Bohemian Rhapsody'),
(6, 'Billie Eilish', '2001-12-18', 'Bad Guy'),
(7, 'Dua Lipa', '1995-08-22', 'Levitating'),
(8, 'The Kid LAROI', '2003-08-17', 'Stay'),
(9, 'Harry Styles', '1994-02-01', 'Watermelon Sugar'),
(10, 'Shawn Mendes', '1998-08-08', 'Senorita'),
(11, 'Camila Cabello', '1997-03-03', 'Havana'),
(12, 'Imagine Dragons', '2008-01-01', 'Radioactive'),
(13, 'OneRepublic', '2002-01-01', 'Counting Stars'),
(14, 'Avicii', '1989-09-08', 'Wake Me Up'),
(15, 'Hozier', '1990-03-17', 'Take Me to Church'),
(16, 'Passenger', '1984-05-17', 'Let Her Go'),
(17, 'Sia', '1975-12-18', 'Cheap Thrills'),
(18, 'Ellie Goulding', '1986-12-30', 'Love Me Like You Do'),
(19, 'Lady Gaga & Bradley Cooper', NULL, 'Shallow'),
(20, 'Post Malone', '1995-07-04', 'Sunflower'),
(21, 'Ariana Grande', '1993-06-26', '7 Rings'),
(22, 'Olivia Rodrigo', '2003-02-20', 'Good 4 U'),
(23, 'Jawsh 685 & Jason Derulo', NULL, 'Savage Love'),
(24, 'BTS', '2013-06-13', 'Dynamite'),
(25, 'Justin Bieber', '1994-03-01', 'Peaches'),
(26, 'Maroon 5', '1994-01-01', 'Memories'),
(27, 'Drake', '1986-10-24', 'God’s Plan'),
(28, 'Juice WRLD', '1998-12-02', 'Lucid Dreams'),
(29, 'Cardi B', '1992-10-11', 'WAP'),
(30, 'DJ Snake', '1986-06-13', 'Taki Taki'),
(31, 'Lil Nas X', '1999-04-09', 'Montero (Call Me by Your Name)'),
(32, 'Elton John', '1947-03-25', 'Rocket Man'),
(33, 'Charlie Puth', '1991-12-02', 'We Don’t Talk Anymore'),
(34, 'Wiz Khalifa', '1987-09-08', 'See You Again'),
(35, 'Tracy Chapman', '1964-03-30', 'Fast Car'),
(36, 'A-ha', '1982-01-01', 'Take On Me'),
(37, 'Guns N’ Roses', '1985-01-01', 'Sweet Child O Mine'),
(38, 'Michael Jackson', '1958-08-29', 'Billie Jean'),
(39, 'Nirvana', '1987-01-01', 'Smells Like Teen Spirit'),
(40, 'Eagles', '1971-01-01', 'Hotel California'),
(41, 'Led Zeppelin', '1968-01-01', 'Stairway to Heaven'),
(42, 'The Beatles', '1960-01-01', 'Hey Jude');

INSERT INTO songs (title, artistId, album, genre, releaseDate) VALUES
('Shape of You', 1, 'Divide', 'Pop', '2017-01-06'),
('Blinding Lights', 2, 'After Hours', 'Synthpop', '2020-11-29'),
('Someone Like You', 3, '21', 'Pop', '2011-01-24'),
('Uptown Funk', 4, 'Uptown Special', 'Funk', '2014-11-10'),
('Bohemian Rhapsody', 5, 'A Night at the Opera', 'Rock', '1975-10-31'),
('Bad Guy', 6, 'When We All Fall Asleep', 'Pop', '2019-03-29'),
('Levitating', 7, 'Future Nostalgia', 'Disco', '2020-03-27'),
('Stay', 8, 'Single', 'Pop', '2021-07-09'),
('Watermelon Sugar', 9, 'Fine Line', 'Pop', '2019-12-13'),
('Senorita', 10, 'Single', 'Latin Pop', '2019-06-21'),
('Rolling in the Deep', 3, '21', 'Pop', '2010-11-29'),
('Havana', 11, 'Camila', 'Latin', '2017-08-03'),
('Radioactive', 12, 'Night Visions', 'Alternative', '2012-04-23'),
('Thunder', 12, 'Evolve', 'Alternative', '2017-05-02'),
('Counting Stars', 13, 'Native', 'Pop Rock', '2013-06-14'),
('Wake Me Up', 14, 'True', 'EDM', '2013-06-17'),
('Take Me to Church', 15, 'Hozier', 'Indie Rock', '2013-09-13'),
('Let Her Go', 16, 'All the Little Lights', 'Folk Rock', '2012-07-24'),
('Thinking Out Loud', 1, 'Multiply', 'Pop', '2014-09-24'),
('Cheap Thrills', 17, 'This Is Acting', 'Electropop', '2016-02-11'),
('Love Me Like You Do', 18, 'Fifty Shades of Grey', 'Pop', '2015-01-07'),
('Shallow', 19, 'A Star Is Born', 'Pop', '2018-09-27'),
('Believer', 12, 'Evolve', 'Rock', '2017-02-01'),
('Perfect', 1, 'Divide', 'Pop', '2017-09-26'),
('Happier', 1, 'Divide', 'Pop', '2017-09-26'),
('Sunflower', 20, 'Spider-Man: Into the Spider-Verse', 'Hip-Hop', '2018-10-18'),
('Rockstar', 20, 'Beerbongs & Bentleys', 'Rap', '2017-09-15'),
('7 Rings', 21, 'Thank U, Next', 'R&B', '2019-01-18'),
('Good 4 U', 22, 'SOUR', 'Pop Rock', '2021-05-14'),
('Drivers License', 22, 'SOUR', 'Pop', '2021-01-08'),
('Savage Love', 23, 'Single', 'Pop', '2020-06-11'),
('Dynamite', 24, 'BE', 'K-Pop', '2020-08-21'),
('Butter', 24, 'Single', 'K-Pop', '2021-05-21'),
('Peaches', 25, 'Justice', 'R&B', '2021-03-19'),
('Memories', 26, 'Jordi', 'Pop', '2019-09-20'),
('Girls Like You', 26, 'Red Pill Blues', 'Pop', '2017-05-30'),
('Beautiful Mistakes', 26, 'Jordi', 'Pop', '2021-03-03'),
('God’s Plan', 27, 'Scorpion', 'Hip-Hop', '2018-01-19'),
('In My Feelings', 27, 'Scorpion', 'Hip-Hop', '2018-07-10'),
('Nice for What', 27, 'Scorpion', 'Hip-Hop', '2018-04-06'),
('Lucid Dreams', 28, 'Goodbye & Good Riddance', 'Hip-Hop', '2018-05-04'),
('Robbery', 28, 'Death Race for Love', 'Hip-Hop', '2019-02-13'),
('WAP', 29, 'Single', 'Hip-Hop', '2020-08-07'),
('Money', 29, 'Invasion of Privacy', 'Hip-Hop', '2018-10-23'),
('Thank U, Next', 21, 'Thank U, Next', 'Pop', '2018-11-03'),
('Break Free', 21, 'My Everything', 'Pop', '2014-07-02'),
('Taki Taki', 30, 'Single', 'Reggaeton', '2018-09-28'),
('I Like It', 29, 'Invasion of Privacy', 'Hip-Hop', '2018-05-25'),
('Industry Baby', 31, 'Montero', 'Hip-Hop', '2021-07-23'),
('Montero (Call Me by Your Name)', 31, 'Montero', 'Hip-Hop', '2021-03-26'),
('Cold Heart', 32, 'The Lockdown Sessions', 'Pop', '2021-08-13'),
('Rocket Man', 32, 'Honky Château', 'Rock', '1972-04-17'),
('Don’t Start Now', 7, 'Future Nostalgia', 'Pop', '2019-11-01'),
('Physical', 7, 'Future Nostalgia', 'Pop', '2020-01-31'),
('IDGAF', 7, 'Dua Lipa', 'Pop', '2017-06-02'),
('New Rules', 7, 'Dua Lipa', 'Pop', '2017-07-21'),
('We Don’t Talk Anymore', 33, 'Nine Track Mind', 'Pop', '2016-01-29'),
('Attention', 33, 'Voicenotes', 'Pop', '2017-04-21'),
('See You Again', 34, 'Furious 7', 'Hip-Hop', '2015-03-10'),
('Fast Car', 35, 'Tracy Chapman', 'Folk Rock', '1988-04-06'),('Take On Me', 36, 'Hunting High and Low', 'Synthpop', '1985-06-01'),
('Sweet Child O Mine', 37, 'Appetite for Destruction', 'Rock', '1987-07-21'),
('Billie Jean', 38, 'Thriller', 'Pop', '1983-01-02'),
('Smells Like Teen Spirit', 39, 'Nevermind', 'Grunge', '1991-09-10'),
('Hotel California', 40, 'Hotel California', 'Rock', '1976-12-08'),
('Stairway to Heaven', 41, 'Led Zeppelin IV', 'Rock', '1971-11-08'),
('Yesterday', 42, 'Help!', 'Pop Rock', '1965-08-06'),
('Hey Jude', 42, 'Single', 'Pop Rock', '1968-08-26');

-- Thêm 5 playlist vào bảng playlists
INSERT INTO playlists (playlistName) VALUES
('Pop Hits'),
('Rock Classics'),
('Hip-Hop Vibes'),
('Latin Beats'),
('Throwback Anthems');

-- Thêm bài hát vào các playlist
-- Pop Hits
INSERT INTO playlist_songs (playlistId, songId) VALUES
(1, 69),  -- Shape of You
(1, 74),  -- Bad Guy
(1, 76),  -- Stay
(1, 71), -- Someone like you
(1, 96); -- 7 Rings

-- Rock Classics
INSERT INTO playlist_songs (playlistId, songId) VALUES
(2, 73),  -- Bohemian Rhapsody
(2, 130), -- Sweet Child O Mine
(2, 133), -- Hotel California
(2, 134), -- Stairway to Heaven
(2, 135); -- Yesterday

-- Hip-Hop Vibes
INSERT INTO playlist_songs (playlistId, songId) VALUES
(3, 95), -- Rockstar
(3, 106), -- God’s Plan
(3, 109), -- Lucid Dreams
(3, 117); -- Industry Baby

-- Latin Beats
INSERT INTO playlist_songs (playlistId, songId) VALUES
(4, 78), -- Senorita
(4, 80), -- Havana
(4, 115), -- Taki Taki
(4, 116), -- I Like It
(4, 102); -- Peaches

-- Throwback Anthems
INSERT INTO playlist_songs (playlistId, songId) VALUES
(5, 129), -- Take On Me
(5, 131), -- Billie Jean
(5, 132), -- Smells Like Teen Spirit
(5, 133), -- Hotel California
(5, 136); -- Hey Jude

SELECT * FROM playlist_songs;

