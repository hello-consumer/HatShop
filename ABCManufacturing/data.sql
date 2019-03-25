INSERT INTO Customer(SalutationTitle, FirstName, LastName) VALUES
('Mr.', 'S.D.', 'Kurtz')

DECLARE @customerId INT
SET @customerId = (SELECT TOP 1 ID FROM Customer)

INSERT INTO [Address](Street, City, State, ZipCode, CustomerID)
VALUES('123 That Street', 'Toronto', 'Ontario', 'A9B 8C7', @customerId),
('456 No Street', 'Hamilton', 'Ontario', 'L6K 5J4', @customerId)

DECLARE @billingId INT
SET @billingId = (SELECT TOP 1 ID FROM Address WHERE Street = '123 That Street')

DECLARE @shippingId INT
SET @shippingId = (SELECT TOP 1 ID FROM Address WHERE Street = '456 No Street')


INSERT INTO [Order](CustomerID, ShippingAddressID, BillingAddressID)
VALUES(@customerId,@shippingId, @billingId)

DECLARE @orderId INT
SET @orderId = (SELECT TOP 1 ID FROM [Order])

INSERT INTO Part(Number, Description, Inventory) VALUES
('WOOD223', '1 X 2 30" wood', 100),
('SCRW110', '1.25" screws', 100),
('WOOD995', '2 X 4 48" wood', 100)

INSERT INTO Product(Code, Description, Inventory, StandardPrice) VALUES
('CH089', 'Patio Chairs', 140, 35),
('FR223', 'Half-Size Refrigerator', 10, 750.99),
('TB101', 'Patio Table', 0, 35)

INSERT INTO ProductPart(ProductCode, PartNumber, QuantityRequired) VALUES
('CH089', 'WOOD223', 8),
('CH089', 'SCRW110', 26),
('TB101', 'WOOD995', 12),
('TB101', 'SCRW110', 34)

INSERT INTO LineItem(OrderID, ProductCode, Quantity, Backordered) VALUES
(@orderId, 'CH089', 20, 0),
(@orderId, 'TB101', 5, 1),
(@orderId, 'FR223', 2, 0)

INSERT INTO PhoneNumber (CustomerID, Number) VALUES
(@customerId,  '(416) 879-0045'),
(@customerId, '(416) 786-3241')

