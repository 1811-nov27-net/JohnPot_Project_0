CREATE SCHEMA PizzaStore

GO
-- Table of all locations
CREATE TABLE PizzaStore.[Location]
(
    LocationId INT IdENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL
)

-- Table of all users
CREATE TABLE PizzaStore.[User]
(
    UserId INT IdENTITY PRIMARY KEY NOT NULL,
    FirstName NVARCHAR(128) NOT NULL,
    LastName NVARCHAR(128) NOT NULL,
    DefaultLocationId INT FOREIGN KEY REFERENCES PizzaStore.[Location] (LocationId)
)

-- Table of all available ingredients
--  (It's the locations job to keep track
--    of how many of each ingredient it has.
--    this is just a table of possible ones
--    it can have and the cost of it.)
CREATE TABLE PizzaStore.Ingredient
(
    IngredientId INT IdENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) UNIQUE NOT NULL,
    -- Cost of ingredient to add to pizza
    Cost FLOAT NOT NULL
)

-- Table of pizzas that have been ordered.
--  Pizza contains ingredients to create za
CREATE TABLE PizzaStore.Pizza
(
    PizzaId INT IdENTITY PRIMARY KEY NOT NULL,
    -- Maximum of 8 toppings per pizza
    IngredientId1 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
    IngredientId2 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
    IngredientId3 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
    IngredientId4 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
    IngredientId5 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
    IngredientId6 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
    IngredientId7 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
    IngredientId8 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientId),
)

-- Table of all placed orders.
CREATE TABLE PizzaStore.[Order]
(
    OrderId    INT IdENTITY PRIMARY KEY NOT NULL,
    LocationId INT FOREIGN KEY REFERENCES PizzaStore.[Location] (LocationId),
    UserId     INT FOREIGN KEY REFERENCES PizzaStore.[User] (UserId),
    TimePlaced DATETIME2 NOT NULL,
    -- Maximum of 12 pizzas per order
    PizzaId1  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId2  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId3  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId4  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId5  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId6  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId7  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId8  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId9  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId10 INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId11 INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId),
    PizzaId12 INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaId)
)

DROP TABLE PizzaStore.[ORDER]
DROP TABLE PizzaStore.Pizza
DROP TABLE PizzaStore.Ingredient
DROP TABLE PizzaStore.[User]
DROP TABLE PizzaStore.[Location]
