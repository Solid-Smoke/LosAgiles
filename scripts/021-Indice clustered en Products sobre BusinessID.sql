USE LosAgilesDB
GO

--Crear copia de Products para probar el índice
CREATE TABLE [dbo].[Products2](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](512) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Price] [int] NOT NULL,
	[Stock] [int] NOT NULL,
	[Weight] [decimal](6, 3) NOT NULL,
	[IsPerishable] [bit] NOT NULL,
	[DailyAmount] [int] NULL,
	[DaysAvailable] [varchar](7) NULL,
	[BusinessID] [int] NOT NULL,
	[ProductImage] [varbinary](max) NULL,
	[Category] [varchar](20) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Products2] ADD  DEFAULT ('No category') FOR [Category]
GO
ALTER TABLE [dbo].[Products2] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Products2]  WITH CHECK ADD FOREIGN KEY([BusinessID])
REFERENCES [dbo].[Businesses] ([BusinessID])
GO
SET IDENTITY_INSERT Products2 ON;
GO
INSERT INTO Products2 ([ProductID], [Description], [Name], [Price], [Stock], [Weight], [IsPerishable], [DailyAmount], [DaysAvailable], [BusinessID], [ProductImage], [Category], [IsDeleted])
SELECT [ProductID], [Description], [Name], [Price], [Stock], [Weight], [IsPerishable], [DailyAmount], [DaysAvailable], [BusinessID], [ProductImage], [Category], [IsDeleted]
FROM Products;
GO
SET IDENTITY_INSERT Products2 OFF;
GO


--Crear el índice
ALTER TABLE [dbo].[ShoppingCarts] DROP CONSTRAINT [FK_ShoppingCarts_ProductID];
ALTER TABLE [dbo].[OrderProducts] DROP CONSTRAINT [FK__OrderProd__Produ__70DDC3D8];
ALTER TABLE [dbo].[Products] DROP CONSTRAINT [PK__Products__B40CC6EDB654FAAB];
CREATE CLUSTERED INDEX [IX_BusinessID] ON [dbo].[Products]
(
	[BusinessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY];
ALTER TABLE [dbo].[Products] ADD CONSTRAINT [PK_Products] PRIMARY KEY NONCLUSTERED ([ProductID]);
ALTER TABLE [dbo].[ShoppingCarts] ADD CONSTRAINT [FK_ShoppingCarts_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products]([ProductID]);
ALTER TABLE [dbo].[OrderProducts] ADD CONSTRAINT [FK__OrderProd__Produ__70DDC3D8] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products]([ProductID]);


--Probar el índice
exec sp_executesql N'SELECT * FROM [dbo].[Products] WHERE [BusinessID] = @BusinessID',N'@BusinessID nvarchar(1)',@BusinessID=N'1'
exec sp_executesql N'SELECT * FROM [dbo].[Products2] WHERE [BusinessID] = @BusinessID',N'@BusinessID nvarchar(1)',@BusinessID=N'1'

--Borrar la tabla copia
DELETE FROM Products2
GO
DROP TABLE Products2
GO
