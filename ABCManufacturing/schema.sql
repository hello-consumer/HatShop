GO

CREATE TABLE [dbo].[Address](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Street] [nvarchar](100) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[State] [nvarchar](100) NOT NULL,
	[ZipCode] [nvarchar](20) NOT NULL,
	[CustomerID] [int] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Customer] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([ID])
GO

ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Customer]
GO

CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SalutationTitle] [nvarchar](100) NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Suffix] [nvarchar](100) NULL,
	[IsPreferred] [bit] NOT NULL,
	[DiscountPercent] [decimal](3, 2) NULL,
	[CreditLimit] [money] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Customer] ADD  DEFAULT ((0)) FOR [IsPreferred]
GO

ALTER TABLE [dbo].[Customer] ADD  DEFAULT ((0)) FOR [DiscountPercent]
GO



CREATE TABLE [dbo].[LineItem](
	[OrderID] [int] NOT NULL,
	[ProductCode] [nvarchar](10) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Backordered] [bit] NOT NULL,
 CONSTRAINT [PK_LineItem] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ProductCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LineItem] ADD  DEFAULT ((0)) FOR [Quantity]
GO

ALTER TABLE [dbo].[LineItem] ADD  DEFAULT ((0)) FOR [Backordered]
GO


CREATE TABLE [dbo].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DiscountedTotal] [money] NOT NULL,
	[Shipped] [bit] NOT NULL,
	[DateOrdered] [datetime] NULL,
	[CustomerID] [int] NOT NULL,
	[BillingAddressID] [int] NOT NULL,
	[ShippingAddressID] [int] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [DiscountedTotal]
GO

ALTER TABLE [dbo].[Order] ADD  DEFAULT ((0)) FOR [Shipped]
GO

ALTER TABLE [dbo].[Order] ADD  DEFAULT (getdate()) FOR [DateOrdered]
GO



CREATE TABLE [dbo].[Part](
	[Number] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Inventory] [int] NOT NULL,
 CONSTRAINT [PK_Part] PRIMARY KEY CLUSTERED 
(
	[Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Part] ADD  DEFAULT ((0)) FOR [Inventory]
GO

CREATE TABLE [dbo].[PhoneNumber](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
	[CustomerID] [int] NOT NULL,
 CONSTRAINT [PK_PhoneNumber] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[Product](
	[Code] [nvarchar](10) NOT NULL,
	[StandardPrice] [money] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Inventory] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Product] ADD  DEFAULT ((0)) FOR [StandardPrice]
GO

ALTER TABLE [dbo].[Product] ADD  DEFAULT ((0)) FOR [Inventory]
GO

CREATE TABLE [dbo].[ProductPart](
	[PartNumber] [nvarchar](20) NOT NULL,
	[ProductCode] [nvarchar](10) NOT NULL,
	[QuantityRequired] [int] NOT NULL,
 CONSTRAINT [PK_ProductPart] PRIMARY KEY CLUSTERED 
(
	[PartNumber] ASC,
	[ProductCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProductPart] ADD  DEFAULT ((0)) FOR [QuantityRequired]
GO

CREATE PROCEDURE [dbo].[SP_OrderReport]
@orderID INT
AS
SELECT
	o.ID, 
	o.DateOrdered, 
	c.FirstName + ' ' + c.LastName as BillingName, 
	b.Street, 
	b.City, 
	b.State, 
	b.ZipCode, 
	s.Street, 
	s.City, 
	s.State, 
	s.ZipCode,
	c.DiscountPercent
FROM 
	[Order] o 
	JOIN [Address] s ON o.ShippingAddressID = s.ID
	JOIN [Address] b ON o.BillingAddressID = b.ID
	JOIN Customer c ON o.CustomerID = c.ID
WHERE 
	o.ID = @orderID

SELECT p.Number FROM PhoneNumber p, Customer c, [Order] o WHERE 
o.ID = @orderID AND o.CustomerID = c.ID

SELECT 
	p.Code, 
	p.Description, 
	l.Quantity, 
	CASE l.Backordered WHEN (1) THEN l.Quantity ELSE 0 END as Backorderd, 
	CASE l.Backordered WHEN (0) THEN l.Quantity ELSE 0 END as Filled, 
	p.StandardPrice,
	l.Quantity * p.StandardPrice as LineTotal
FROM 
	[Order] o, LineItem l, Product p 
WHERE 
	o.ID = l.OrderID AND l.ProductCode = p.Code

DECLARE @subtotal MONEY

SELECT 
	SUM(l.Quantity * p.StandardPrice) AS Subtotal, 
	SUM(c.DiscountPercent * l.Quantity * p.StandardPrice) AS Discount,
	SUM(l.Quantity * p.StandardPrice) - SUM(c.DiscountPercent * l.Quantity * p.StandardPrice) AS Total
FROM 
	[Order] o, LineItem l, Product p, Customer c 
WHERE 
	o.ID = l.OrderID AND l.ProductCode = p.Code AND c.ID = o.CustomerID

SELECT 
	p.Code, 
	p.Description,
	p.Inventory,
	IIF(p.Inventory - l.Quantity < 0, l.Quantity - p.Inventory, 0) AS BackOrdered,
	p.StandardPrice
FROM 
	[Order] o, LineItem l, Product p 
WHERE 
	o.ID = l.OrderID AND l.ProductCode = p.Code

SELECT 
	Product.Code, Product.Description,
	Part.Number, Part.Description, ProductPart.QuantityRequired
FROM 

	[Order] o, LineItem l, Product, ProductPart, Part
WHERE 
	o.ID = l.OrderID 
	AND l.ProductCode = Product.Code 
	AND Product.Code = ProductPart.ProductCode 
	AND Part.Number = ProductPart.PartNumber
ORDER BY Product.Code ASC
GO
