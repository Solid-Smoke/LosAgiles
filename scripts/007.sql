USE LosAgilesDB;
GO

INSERT INTO Clients (Name, LastNames, UserName, Email, BirthDate, UserPassword, AccountState)
VALUES  ('Maria','Rodriguez','@maria123','maria@gmail.com','2000-05-01','1234','Activo'),
		('Jose','Rodriguez','@jose123','jose@gmail.com','1995-04-10','1234','Activo'),
		('Juan','Rodriguez','@juan123','juan@gmail.com','1999-09-30','1234','Inactivo'),
		('Antonio','Rodriguez','@antonio123','antonio@gmail.com','1990-12-24','1234','Bloqueado'),
		('David','Rodriguez','@david123','david@gmail.com','1997-04-10','1234','Activo');
GO

INSERT INTO ClientsAddresses (UserID, Province, Canton, District, PostalCode, OtherSigns)
VALUES 
(1, 'San José', 'Central', 'Carmen', 10101, 'Cerca del parque central'),
(2, 'Alajuela', 'Alajuela', 'San Rafael', 20101, 'Frente a la iglesia principal'),
(3, 'Heredia', 'Heredia', 'San Francisco', 30101, 'Detrás del supermercado X');
GO

INSERT INTO Businesses (Name, IDNumber, Email, Telephone, Permissions)
VALUES 
('Tech Solutions', '3101234567', 'info@techsolutions.com', '2222-3333', 'Venta de software, Consultoría'),
('Green Farms', '3107654321', 'contact@greenfarms.com', '2222-4444', 'Producción de alimentos orgánicos'),
('Digital Marketing Co', '3109876543', 'sales@digitalmkt.com', '2222-5555', 'Servicios de marketing digital');
GO

INSERT INTO BusinessesAddresses (BusinessID, Province, Canton, District, PostalCode, OtherSigns)
VALUES 
(1, 'San José', 'Escazú', 'San Rafael', '10201', 'A 100 metros del centro comercial'),
(2, 'Alajuela', 'San Carlos', 'Quesada', '20201', 'Cerca del banco estatal'),
(3, 'Heredia', 'Belén', 'La Ribera', '30201', 'Frente a la zona industrial');
GO

INSERT INTO Employees (BusinessID, UserID)
VALUES 
(1, 1),
(2, 2),
(3, 3);
GO