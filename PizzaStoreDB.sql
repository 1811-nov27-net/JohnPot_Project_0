CREATE SCHEMA PizzaStore

GO
-- Table of all available ingredients and 
--  their respective costs
CREATE TABLE PizzaStore.Ingredient
(
    IngredientId INT IDENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) UNIQUE NOT NULL,
    -- Cost of ingredient to add to pizza
    Cost FLOAT NOT NULL 
)

-- Table of all locations
CREATE TABLE PizzaStore.[Location]
(
    LocationId INT IDENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL
)

CREATE TABLE PizzaStore.Inventory
(
    LocationId INT FOREIGN KEY REFERENCES PizzaStore.[Location] (LocationId),
    IngredientId INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID),
    [Count] INT,
    CONSTRAINT PK_Inventory PRIMARY KEY (LocationId, IngredientId)
)

-- Table of all users
CREATE TABLE PizzaStore.[User]
(
    UserId INT IDENTITY PRIMARY KEY NOT NULL,
    FirstName NVARCHAR(128) NOT NULL,
    LastName NVARCHAR(128) NOT NULL,
    DefaultLocationId INT FOREIGN KEY REFERENCES PizzaStore.[Location] (LocationId)
)

-- Table of pizzas that have been ordered.
CREATE TABLE PizzaStore.Pizza
(
    PizzaId INT,
    IngredientId INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
    [Count] INT,
    CONSTRAINT PK_Pizza PRIMARY KEY (PizzaId, IngredientId)
)

-- Table of all placed orders.
CREATE TABLE PizzaStore.[Order]
(
    OrderId    INT NOT NULL,
    LocationId INT FOREIGN KEY REFERENCES PizzaStore.[Location] (LocationId),
    UserId     INT FOREIGN KEY REFERENCES PizzaStore.[User] (UserId),
    TimePlaced DATETIME2 NOT NULL,
    PizzaId    INT,
    CONSTRAINT PK_Order PRIMARY KEY (OrderId, PizzaId)
)

DROP TABLE PizzaStore.[Order]
DROP TABLE PizzaStore.[User]
DROP TABLE PizzaStore.Pizza
DROP TABLE PizzaStore.Inventory
DROP TABLE PizzaStore.[Location]
DROP TABLE PizzaStore.Ingredient


SELECT * FROM PizzaStore.[User]

SELECT * FROM PizzaStore.[Location]

SELECT * FROM PizzaStore.Inventory

SELECT * FROM PizzaStore.Ingredient

SELECT * FROM PizzaStore.[Order]

SELECT * FROM PizzaStore.Pizza

-- Base list of ingredients and prices
INSERT INTO PizzaStore.Ingredient(Name, Cost)
    VALUES('cheese', 0.5)
INSERT INTO PizzaStore.Ingredient(Name, Cost)
    VALUES('olive', 1.0)
INSERT INTO PizzaStore.Ingredient(Name, Cost)
    VALUES('pepperoni', 1.0)
INSERT INTO PizzaStore.Ingredient(Name, Cost)
    VALUES('pineapple', 2.5)
INSERT INTO PizzaStore.Ingredient(Name, Cost)
    VALUES('onion', 3.5)
INSERT INTO PizzaStore.Ingredient(Name, Cost)
    VALUES('sausage', 4)
INSERT INTO PizzaStore.Ingredient(Name, Cost)
    VALUES('bacon', 10)