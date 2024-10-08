USE LosAgilesDB;
GO

CREATE TABLE Clients (
    UserID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    [Name] VARCHAR(30) NOT NULL,
    LastNames VARCHAR(60) NOT NULL,
    UserName VARCHAR(30) NOT NULL,
    Email VARCHAR(60) NOT NULL,
    BirthDate DATE NOT NULL,
    UserPassword VARCHAR(512) NOT NULL,
    AccountState VARCHAR(10) NOT NULL DEFAULT 'Inactivo',
    Rol VARCHAR(12) NOT NULL DEFAULT 'Comprador',
    UNIQUE(UserName, Email)
);
GO
