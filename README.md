frontend:    git clone https://github.com/JesusMorquecho/prueba-tecnica-frontend.git

script base de datos:
CREATE DATABASE MoviesDB;
GO

USE MoviesDB;
GO

CREATE TABLE Director (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Name VARCHAR(200) NOT NULL,
    Nationality VARCHAR(100),
    Age INT,
    Active BIT
);
GO

CREATE TABLE Movies (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Name VARCHAR(100) NOT NULL,
    ReleaseYear DATE,
    Gender VARCHAR(50),
    Duration TIME,
    FKDirector INT,
    FOREIGN KEY (FKDirector) REFERENCES Director(Id)
);
GO
SELECT 
    m.Id AS MovieId,
    m.Name AS MovieName,
    m.ReleaseYear,
    m.Gender,
    m.Duration,
    d.Id AS DirectorId,
    d.Name AS DirectorName,
    d.Nationality,
    d.Age,
    d.Active
FROM 
    Movies m
INNER JOIN 
    Director d
ON 
    m.FKDirector = d.Id;
