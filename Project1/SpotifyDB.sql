DROP DATABASE IF EXISTS SpotifyDB;

CREATE DATABASE SpotifyDB;

USE SpotifyDB;

CREATE TABLE users(
    userId INT PRIMARY KEY AUTO_INCREMENT,
    userName VARCHAR(50) NOT NULL,
    userEmail VARCHAR(50) NOT NULL,
    userPassword VARCHAR(50) NOT NULL,
    roles VARCHAR(50) NOT NULL
);

CREATE TABLE artists(
    artistId INT PRIMARY KEY AUTO_INCREMENT,
    artistName VARCHAR(100) NOT NULL,
    birthDate DATE,
    topSong VARCHAR(100)
);

CREATE TABLE songs(
    songId INT PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(100) NOT NULL,
    artistId INT,
    album VARCHAR(100),
    genre VARCHAR(50), 
    releaseDate DATE,
    FOREIGN KEY (artistId) REFERENCES artists(artistId) ON DELETE SET NULL
);

CREATE TABLE playlists(
    playlistId INT PRIMARY KEY AUTO_INCREMENT,
    playlistName VARCHAR(100) NOT NULL,
    createdAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE playlistSongs(
    playlistId INT NOT NULL,
    songId INT NOT NULL,
    PRIMARY KEY(playlistId, songId),
    FOREIGN KEY(playlistId) REFERENCES playlists(playlistId) ON DELETE CASCADE,
    FOREIGN KEY(songId) REFERENCES songs(songId) ON DELETE CASCADE
);

CREATE TABLE likedSongs(
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

CREATE TABLE myPlaylistSongs(
    myPlaylistId INT NOT NULL,
    songId INT NOT NULL,
    PRIMARY KEY (myPlaylistId, songId),
    FOREIGN KEY (myPlaylistId) REFERENCES myPlaylist(myPlaylistId) ON DELETE CASCADE,
    FOREIGN KEY (songId) REFERENCES songs(songId) ON DELETE CASCADE
);