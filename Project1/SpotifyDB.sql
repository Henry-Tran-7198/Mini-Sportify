-- DROP DATABASE IF EXISTS SpotifyDB; (in case when broken...)

CREATE DATABASE SpotifyDB;

USE SpotifyDB;

CREATE TABLE users(
    userId INT PRIMARY KEY AUTO_INCREMENT,
    userName VARCHAR(50) NOT NULL,
    userEmail VARCHAR(50) NOT NULL,
    userPassword VARCHAR(50) NOT NULL,
    roles VARCHAR(50) NOT NULL
);
