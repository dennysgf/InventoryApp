
CREATE DATABASE InventoryDB;
GO
USE InventoryDB;
GO

CREATE TABLE Products (
                          Id INT IDENTITY(1,1) PRIMARY KEY,
                          Name NVARCHAR(100) NOT NULL,
                          Description NVARCHAR(255) NULL,
                          Category NVARCHAR(100) NULL,
                          ImageUrl NVARCHAR(255) NULL,
                          Price DECIMAL(18,2) NOT NULL,
                          Stock INT NOT NULL
);
GO

CREATE TABLE Transactions (
                              Id INT IDENTITY(1,1) PRIMARY KEY,
                              Date DATETIME NOT NULL DEFAULT GETDATE(),
                              Type NVARCHAR(50) NOT NULL,
                              ProductId INT NOT NULL,
                              Quantity INT NOT NULL,
                              UnitPrice DECIMAL(18,2) NOT NULL,
                              TotalPrice AS (Quantity * UnitPrice) PERSISTED,
                              Detail NVARCHAR(255) NULL,
                              CONSTRAINT FK_Transactions_Products FOREIGN KEY (ProductId)
                                  REFERENCES Products(Id)
);
GO
