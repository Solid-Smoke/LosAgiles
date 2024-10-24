USE LosAgilesDB;
GO

ALTER TABLE Products
ADD ProductImage VARBINARY(MAX);
GO

sp_rename 'Products.Perishable', 'IsPerishable', 'COLUMN';
GO

ALTER TABLE Products
ADD Category VARCHAR(20) DEFAULT 'No category';
GO