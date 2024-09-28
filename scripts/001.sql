use master

CREATE DATABASE LosAgilesDB

use LosAgilesDB

CREATE TABLE Clients (
    UserID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    Name VARCHAR(30),
    LastNames VARCHAR(60),
    UserName VARCHAR(30),
    Email VARCHAR(60),
    BirthDate DATE,
    UserPassword VARCHAR(512)
);

CREATE TABLE ClientsAddresses (
    AddressID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Clients(UserID),
    Province VARCHAR(20),
    Canton VARCHAR(40),
    District VARCHAR(50),
    PostalCode INT,
    OtherSigns VARCHAR(500)
);

