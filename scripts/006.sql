USE LosAgilesDB
GO;

CREATE TABLE Employees (
	BusinessID INT NOT NULL,                  
    UserID INT NOT NULL,
	PRIMARY KEY (BusinessID, UserID), 
    CONSTRAINT FK_BusinessEmployee FOREIGN KEY (BusinessID) REFERENCES Businesses(BusinessID),
	CONSTRAINT FK_ClientEmployee FOREIGN KEY (UserID) REFERENCES Clients(UserID)
);
GO;