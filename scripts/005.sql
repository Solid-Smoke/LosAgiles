USE LosAgilesDB
GO;

CREATE TABLE SuperUsers (
	SuperUserID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	UserName VARCHAR(30) NOT NULL,
	UserPassword VARCHAR(512) NOT NULL,
	CreatedByID INT NOT NULL
);
GO;

INSERT INTO [dbo].SuperUsers([UserName], [UserPassword], CreatedByID)
VALUES  ('FirstAdmin','AwesomeSauce','1')
GO;

ALTER TABLE SuperUsers ADD CONSTRAINT FK_SelfCreatedbySU
FOREIGN KEY (CreatedByID) REFERENCES SuperUsers(SuperUserID);
GO;