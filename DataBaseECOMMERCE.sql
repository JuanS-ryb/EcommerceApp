

CREATE DATABASE EcommerceApp;
GO

USE EcommerceApp;
GO

-- ------------------------------------------------------------
-- Users
-- ------------------------------------------------------------
CREATE TABLE Users (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Name          NVARCHAR(150)   NOT NULL,
    Email         NVARCHAR(150)   NOT NULL UNIQUE,
    PasswordHash  NVARCHAR(256)   NOT NULL,
    CreatedAt     DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ------------------------------------------------------------
-- Products
-- ------------------------------------------------------------
CREATE TABLE Products (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Name         NVARCHAR(150)   NOT NULL,
    Description  NVARCHAR(500)   NULL,
    Price        DECIMAL(12,2)   NOT NULL CHECK (Price >= 0),
    ImageUrl     NVARCHAR(300)   NULL,
    Stock        INT             NOT NULL DEFAULT 0 CHECK (Stock >= 0),
    CreatedAt    DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ------------------------------------------------------------
-- CartItems (carrito persistido por usuario)
-- ------------------------------------------------------------
CREATE TABLE CartItems (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    UserId     INT NOT NULL,
    ProductId  INT NOT NULL,
    Quantity   INT NOT NULL CHECK (Quantity > 0),
    CreatedAt  DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_CartItems_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CartItems_Products
        FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,

    -- Un mismo producto no debería repetirse como fila distinta en el carrito de un usuario
    CONSTRAINT UQ_CartItems_User_Product UNIQUE (UserId, ProductId)
);
GO

-- ------------------------------------------------------------
-- Orders (cabecera del pedido)
-- ------------------------------------------------------------
CREATE TABLE Orders (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    UserId     INT NOT NULL,
    OrderDate  DATETIME2      NOT NULL DEFAULT SYSUTCDATETIME(),
    Total      DECIMAL(12,2)  NOT NULL CHECK (Total >= 0),
    Status     NVARCHAR(30)   NOT NULL DEFAULT 'Completed',
        -- valores esperados: Completed | Cancelled (no se requieren pagos reales)

    CONSTRAINT FK_Orders_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO

-- ------------------------------------------------------------
-- OrderItems (detalle del pedido)
-- ------------------------------------------------------------
CREATE TABLE OrderItems (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    OrderId    INT NOT NULL,
    ProductId  INT NOT NULL,
    Quantity   INT NOT NULL CHECK (Quantity > 0),
    UnitPrice  DECIMAL(12,2) NOT NULL CHECK (UnitPrice >= 0),
        -- se copia el precio del producto al momento de la compra,
        -- para que el histórico no cambie si el precio del producto cambia después

    CONSTRAINT FK_OrderItems_Orders
        FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Products
        FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO

-- ------------------------------------------------------------
-- (Opcional) Tablas para la "Prueba de análisis" de puntos,
-- por si quieres exponerla también como endpoint del backend
-- en vez de dejarla solo como cálculo en el frontend.
-- ------------------------------------------------------------
CREATE TABLE SalesGoals (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT NOT NULL,
    Quarter         TINYINT NOT NULL CHECK (Quarter BETWEEN 1 AND 4),
    Year            SMALLINT NOT NULL,
    PesosGoal       DECIMAL(14,2) NOT NULL DEFAULT 11000000,
    UnitsGoal       INT           NOT NULL DEFAULT 6000,
    ExecutedPesos   DECIMAL(14,2) NOT NULL DEFAULT 0,
    ExecutedUnits   INT           NOT NULL DEFAULT 0,

    CONSTRAINT FK_SalesGoals_Users
        FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT UQ_SalesGoals_User_Period UNIQUE (UserId, Quarter, Year)
);
GO

-- ------------------------------------------------------------
-- Índices recomendados
-- ------------------------------------------------------------
CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_CartItems_UserId ON CartItems(UserId);
GO

-- ------------------------------------------------------------
-- Datos semilla: los 3 productos del catálogo
-- ------------------------------------------------------------
INSERT INTO Products (Name, Description, Price, ImageUrl, Stock) VALUES
('Audífonos Inalámbricos', 'Audífonos bluetooth con cancelación de ruido.', 120000, 'https://picsum.photos/seed/audifonos/400/300', 50),
('Reloj Inteligente', 'Smartwatch con monitor de ritmo cardíaco.', 250000, 'https://picsum.photos/seed/reloj/400/300', 30),
('Mochila Antirrobo', 'Mochila resistente al agua con puerto USB.', 95000, 'https://picsum.photos/seed/mochila/400/300', 40);
GO



-- dotnet ef dbcontext scaffold "Server=(localdb)\MSSQLLocalDB;Database=EcommerceApp;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models --context AppDbContext --context-dir Context --project Ecommerce.server.csproj --force