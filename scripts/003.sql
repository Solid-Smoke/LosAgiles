USE LosAgilesDB

CREATE TABLE ClientsAddresses (
    AddressID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Clients(UserID),
    Province VARCHAR(20) NOT NULL,
    Canton VARCHAR(40) NOT NULL,
    District VARCHAR(50) NOT NULL,
    PostalCode INT NOT NULL,
    OtherSigns VARCHAR(500) NOT NULL
);
go

CREATE TABLE Businesses (
    BusinessID INT NOT NULL PRIMARY KEY IDENTITY(1,1),      
    Name NVARCHAR(50) NOT NULL,              
    IDNumber NVARCHAR(20) NOT NULL,           
    Email NVARCHAR(50) NOT NULL,             
    Telephone NVARCHAR(20) NOT NULL,          
    Permissions NVARCHAR(200),         
);
go

CREATE TABLE BusinessesAddresses (
    BusinessID INT NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES Businesses(BusinessID),
    Province NVARCHAR(20) NOT NULL,
    Canton NVARCHAR(40) NOT NULL,
    District NVARCHAR(40) NOT NULL,
    PostalCode NVARCHAR(10) NOT NULL,
    OtherSigns VARCHAR(500) NOT NULL
);
go