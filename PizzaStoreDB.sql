--DROP TABLE [User]

-- Table of all users
CREATE TABLE [User]
(
    UserID INT IDENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    DefaultLocationID INT FOREIGN KEY REFERENCES [Location] (LocationID)
)

-- Table of all locations
CREATE TABLE Locaiton
(
    LocationID INT IDENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL
)

-- Table of all available ingredients
--  (It's the locations job to keep track
--    of how many of each ingredient it has.
--    this is just a table of possible ones
--    it can have and the cost of it.)
CREATE TABLE Ingredient
(
    IngredientID INT IDENTITY PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    -- Cost of ingredient to add to pizza
    Cost FLOAT NOT NULL
)

-- Table of pizzas that have been ordered.
--  Pizza contains ingredients to create za
CREATE TABLE Pizza
(
    PizzaID INT IDENTITY PRIMARY KEY NOT NULL,
    -- Maximum of 8 toppings per pizza
    IngredientID1 INT FOREIGN KEY REFERENCES Ingredient (IngredientID),
    IngredientID2 INT FOREIGN KEY REFERENCES Ingredient (IngredientID),
    IngredientID3 INT FOREIGN KEY REFERENCES Ingredient (IngredientID),
    IngredientID4 INT FOREIGN KEY REFERENCES Ingredient (IngredientID),
    IngredientID5 INT FOREIGN KEY REFERENCES Ingredient (IngredientID),
    IngredientID6 INT FOREIGN KEY REFERENCES Ingredient (IngredientID),
    IngredientID7 INT FOREIGN KEY REFERENCES Ingredient (IngredientID),
    IngredientID8 INT FOREIGN KEY REFERENCES Ingredient (IngredientID)
)

--DROP TABLE [ORDER]
-- Table of all placed orders.
CREATE TABLE [Order]
(
    OrderID    INT IDENTITY PRIMARY KEY NOT NULL,
    LocationID INT FOREIGN KEY REFERENCES Location (LocationID),
    UserID     INT FOREIGN KEY REFERENCES [Order] (OrderID),
    TimePlaced DATETIME2 NOT NULL,
    -- Maximum of 12 pizzas per order
    PizzaID1  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID2  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID3  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID4  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID5  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID6  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID7  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID8  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID9  INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID10 INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID11 INT FOREIGN KEY REFERENCES Pizza (PizzaID),
    PizzaID12 INT FOREIGN KEY REFERENCES Pizza (PizzaID)
)

