USE LosAgilesDB;
GO

CREATE TABLE Products (
	ProductID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	Description VARCHAR(512) NOT NULL,
	Name VARCHAR(50) NOT NULL,
	Price INT NOT NULL,
	Stock INT NOT NULL,
	Weight DECIMAL(6,3) NOT NULL,
	Perishable BIT NOT NULL,
	DailyAmount INT,
	DaysAvailable VARCHAR(7),
	BusinessID INT NOT NULL,
	FOREIGN KEY (BusinessID) REFERENCES Businesses(BusinessID)
);
GO

CREATE TABLE Images (
	ImageID INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	Image VARBINARY(MAX) NOT NULL,
	ProductID INT NOT NULL,
	FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);
GO
