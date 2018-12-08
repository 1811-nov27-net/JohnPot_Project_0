CREATE SCHEMA PizzaStore

GO
--DROP TABLE PizzaStore.[Location]
-- Table of all locations
CREATE TABLE PizzaStore.[Location]
(
    LocationID INT IDENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL
)

--DROP TABLE [User]
-- Table of all users
CREATE TABLE PizzaStore.[User]
(
    UserID INT IDENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    DefaultLocationID INT FOREIGN KEY REFERENCES PizzaStore.[Location] (LocationID)
)

-- Table of all available ingredients
--  (It's the locations job to keep track
--    of how many of each ingredient it has.
--    this is just a table of possible ones
--    it can have and the cost of it.)
--DROP TABLE PizzaStore.Ingredient
CREATE TABLE PizzaStore.Ingredient
(
    IngredientID INT IDENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    -- Cost of ingredient to add to pizza
    Cost FLOAT NOT NULL
)

-- Table of pizzas that have been ordered.
--  Pizza contains ingredients to create za
CREATE TABLE PizzaStore.Pizza
(
    PizzaID INT IDENTITY PRIMARY KEY NOT NULL,
    -- Maximum of 8 toppings per pizza
    IngredientID1 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID),
    IngredientID2 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID),
    IngredientID3 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID),
    IngredientID4 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID),
    IngredientID5 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID),
    IngredientID6 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID),
    IngredientID7 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID),
    IngredientID8 INT FOREIGN KEY REFERENCES PizzaStore.Ingredient (IngredientID)
)

--DROP TABLE [ORDER]
-- Table of all placed orders.
CREATE TABLE PizzaStore.[Order]
(
    OrderID    INT IDENTITY PRIMARY KEY NOT NULL,
    LocationID INT FOREIGN KEY REFERENCES PizzaStore.[Location] (LocationID),
    UserID     INT FOREIGN KEY REFERENCES PizzaStore.[User] (UserID),
    TimePlaced DATETIME2 NOT NULL,
    -- Maximum of 12 pizzas per order
    PizzaID1  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID2  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID3  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID4  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID5  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID6  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID7  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID8  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID9  INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID10 INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID11 INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID),
    PizzaID12 INT FOREIGN KEY REFERENCES PizzaStore.Pizza (PizzaID)
)

