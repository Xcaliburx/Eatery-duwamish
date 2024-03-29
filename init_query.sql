USE [EateryDB]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_General_Split]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_General_Split]
(
	@list VARCHAR(MAX),
	@delimiter VARCHAR(5)
)
RETURNS @retVal TABLE (Id INT IDENTITY(1,1), Value VARCHAR(MAX))
AS
BEGIN
	WHILE (CHARINDEX(@delimiter, @list) > 0)
	BEGIN
		INSERT INTO @retVal (Value)
		SELECT Value = LTRIM(RTRIM(SUBSTRING(@list, 1, CHARINDEX(@delimiter, @list) - 1)))
		SET @list = SUBSTRING(@list, CHARINDEX(@delimiter, @list) + LEN(@delimiter), LEN(@list))
	END
	INSERT INTO @retVal (Value)
	SELECT Value = LTRIM(RTRIM(@list))
	RETURN 
END
GO
/****** Object:  Table [dbo].[msDish]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msDish](
	[DishID] [int] IDENTITY(1,1) NOT NULL,
	[DishTypeID] [int] NOT NULL,
	[DishName] [varchar](200) NOT NULL,
	[DishPrice] [int] NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DishID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[msDishType]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msDishType](
	[DishTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DishTypeName] [varchar](100) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DishTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[msDish]  WITH CHECK ADD FOREIGN KEY([DishTypeID])
REFERENCES [dbo].[msDishType] ([DishTypeID])
GO
/****** Object:  StoredProcedure [dbo].[Dish_Delete]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Delete dish
 */
CREATE PROCEDURE [dbo].[Dish_Delete]
	@DishIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msDish
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE DishID IN (SELECT value FROM fn_General_Split(@DishIDs, ','))
END
GO
/****** Object:  StoredProcedure [dbo].[Dish_Get]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get semua dish
 */
CREATE PROCEDURE [dbo].[Dish_Get]
AS
BEGIN
	SELECT 
		DishID,
		DishTypeID,
		DishName, 
		DishPrice 
	FROM msDish WITH(NOLOCK)
	WHERE AuditedActivity <> 'D'
END
GO
/****** Object:  StoredProcedure [dbo].[Dish_GetByID]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get dish tertentu by Id
 */
CREATE PROCEDURE [dbo].[Dish_GetByID]
	@DishId INT
AS
BEGIN
	SELECT 
		DishID,
		DishTypeID,
		DishName, 
		DishPrice 
	FROM msDish WITH(NOLOCK)
	WHERE DishId = @DishId AND AuditedActivity <> 'D'
END
GO
/****** Object:  StoredProcedure [dbo].[Dish_InsertUpdate]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Insert atau update dish
 */
CREATE PROCEDURE [dbo].[Dish_InsertUpdate]
	@DishID INT OUTPUT,
	@DishTypeID INT,
	@DishName VARCHAR(100),
	@DishPrice INT
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msDish WITH(NOLOCK) WHERE DishID = @DishID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msDish
		SET DishName = @DishName,
			DishTypeID = @DishTypeID,
			DishPrice = @DishPrice,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE DishID = @DishID AND AuditedActivity <> 'D'
		SET @RetVal = @DishID
	END
	ELSE
	BEGIN
		INSERT INTO msDish 
		(DishName, DishTypeID, DishPrice, AuditedActivity, AuditedTime)
		VALUES
		(@DishName, @DishTypeID, @DishPrice, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @DishId = @RetVal
END
GO
/****** Object:  StoredProcedure [dbo].[DishType_Get]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get semua dish type
 */
CREATE PROCEDURE [dbo].[DishType_Get]
AS
BEGIN
	SELECT DishTypeID, DishTypeName FROM msDishType WITH(NOLOCK) 
	WHERE AuditedActivity <> 'D'
END
GO
/****** Object:  StoredProcedure [dbo].[DishType_GetByID]    Script Date: 20/05/2021 7:23:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Jonathan Ibrahim
 * Date: 10 Mar 2021
 * Purpose: Get dish type by ID
 */
CREATE PROCEDURE [dbo].[DishType_GetByID]
	@DishTypeID INT
AS
BEGIN
	SELECT DishTypeID, DishTypeName
	FROM msDishType WITH(NOLOCK)
	WHERE DishTypeID = @DishTypeID AND AuditedActivity <> 'D'
END
GO
-- SEEDING msDishType
INSERT INTO msDishType (DishTypeName,AuditedActivity,AuditedTime)
VALUES ('Rumahan','I',GETDATE()), ('Restoran','I',GETDATE()), ('Pinggiran','I',GETDATE())

/****** Object:  Table [dbo].[msRecipe]    Script Date: 24/06/2021 5:00:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msRecipe](
	[RecipeID] [int] IDENTITY(1,1) NOT NULL,
	[DishID] [int] NOT NULL,
	[RecipeName] [varchar](100) NOT NULL,
	[RecipeDescription] [varchar](255) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecipeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[msRecipe]  WITH CHECK ADD FOREIGN KEY([DishID])
REFERENCES [dbo].[msDish] ([DishID])
GO

/****** Object:  Table [dbo].[msIngredients]    Script Date: 24/06/2021 5:10:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[msIngredients](
	[IngredientID] [int] IDENTITY(1,1) NOT NULL,
	[RecipeID] [int] NOT NULL,
	[IngredientName] [varchar](100) NOT NULL,
	[IngredientQuantity] [int] NOT NULL,
	[IngredientUnit] [varchar](100) NOT NULL,
	[AuditedActivity] [char](1) NOT NULL,
	[AuditedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IngredientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[msIngredients]  WITH CHECK ADD FOREIGN KEY([IngredientID])
REFERENCES [dbo].[msRecipe] ([RecipeID])
GO

/****** Object:  StoredProcedure [dbo].[Recipes_Delete]    Script Date: 24/06/2021 5:22:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Benny Kharisma
 * Date: 24 June 2021
 * Purpose: Delete Recipes
 */
CREATE PROCEDURE [dbo].[Recipes_Delete]
	@RecipeIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msRecipe
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE RecipeID IN (SELECT value FROM fn_General_Split(@RecipeIDs, ','))
END
GO

/****** Object:  StoredProcedure [dbo].[Recipes_GetByDishID]    Script Date: 24/06/2021 5:28:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Benny Kharisma
 * Date: 24 June 2021
 * Purpose: Get recipe tertentu by DishID
 */
CREATE PROCEDURE [dbo].[Recipes_GetByDishID]
	@DishId INT
AS
BEGIN
	SELECT 
		RecipeID,
		RecipeName
	FROM msRecipe WITH(NOLOCK)
	WHERE DishId = @DishId AND AuditedActivity <> 'D'
END
GO

/****** Object:  StoredProcedure [dbo].[Recipe_InsertUpdate]    Script Date: 24/06/2021 5:32:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Benny Kharisma
 * Date: 24 June 2021
 * Purpose: Insert and Update recipe
 */
CREATE PROCEDURE [dbo].[Recipe_InsertUpdate]
	@RecipeID INT OUTPUT,
	@RecipeName VARCHAR(100),
	@RecipeDesc VARCHAR(255),
	@DishID INT
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msRecipe WITH(NOLOCK) WHERE RecipeID = @RecipeID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msRecipe
		SET RecipeName = @RecipeName,
			DishID = @DishID,
			RecipeDescription = @RecipeDesc,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE RecipeID = @RecipeID AND AuditedActivity <> 'D'
		SET @RetVal = @RecipeID
	END
	ELSE
	BEGIN
		INSERT INTO msRecipe 
		(DishID, RecipeName, RecipeDescription, AuditedActivity, AuditedTime)
		VALUES
		(@DishID , @RecipeName, @RecipeDesc, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @RecipeID = @RetVal
END
GO

/****** Object:  StoredProcedure [dbo].[Recipes_GetByID]    Script Date: 24/06/2021 5:52:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Benny Kharisma
 * Date: 24 June 2021
 * Purpose: Get recipe tertentu by ID
 */
CREATE PROCEDURE [dbo].[Recipes_GetByID]
	@RecipeId INT
AS
BEGIN
	SELECT 
		RecipeID,
		RecipeName,
		RecipeDescription,
		DishID
	FROM msRecipe WITH(NOLOCK)
	WHERE RecipeID = @RecipeId AND AuditedActivity <> 'D'
END
GO

/****** Object:  StoredProcedure [dbo].[Ingredients_Delete]    Script Date: 24/06/2021 5:55:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Benny Kharisma
 * Date: 24 June 2021
 * Purpose: Delete Ingredients
 */
CREATE PROCEDURE [dbo].[Ingredients_Delete]
	@IngredientsIDs VARCHAR(MAX)
AS
BEGIN
	UPDATE msIngredients
	SET AuditedActivity = 'D',
		AuditedTime = GETDATE()
	WHERE IngredientID IN (SELECT value FROM fn_General_Split(@IngredientsIDs, ','))
END
GO

/****** Object:  StoredProcedure [dbo].[Ingredients_GetByRecipeID]    Script Date: 24/06/2021 6:00:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Benny Kharisma
 * Date: 24 June 2021
 * Purpose: Get ingredients tertentu by ID
 */
CREATE PROCEDURE [dbo].[Ingredients_GetByRecipeID]
	@RecipeId INT
AS
BEGIN
	SELECT 
		IngredientID,
		IngredientName,
		IngredientQuantity,
		IngredientUnit
	FROM msIngredients WITH(NOLOCK)
	WHERE RecipeID = @RecipeId AND AuditedActivity <> 'D'
END
GO

/****** Object:  StoredProcedure [dbo].[Ingredients_GetByID]    Script Date: 24/06/2021 6:05:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Benny Kharisma
 * Date: 24 June 2021
 * Purpose: Get ingredient tertentu by ID
 */
CREATE PROCEDURE [dbo].[Ingredients_GetByID]
	@IngredientId INT
AS
BEGIN
	SELECT 
		IngredientID,
		IngredientName,
		IngredientQuantity,
		IngredientUnit
	FROM msIngredients WITH(NOLOCK)
	WHERE IngredientID = @IngredientId AND AuditedActivity <> 'D'
END
GO

/****** Object:  StoredProcedure [dbo].[Ingredient_InsertUpdate]    Script Date: 24/06/2021 6:09:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/**
 * Created by: Benny Kharisma
 * Date: 24 June 2021
 * Purpose: Insert and Update ingredient
 */
CREATE PROCEDURE [dbo].[Ingredient_InsertUpdate]
	@IngredientID INT OUTPUT,
	@IngredientName VARCHAR(100),
	@IngredientQuantity INT,
	@IngredientUnit VARCHAR(100),
	@RecipeID INT
AS
BEGIN
	DECLARE @RetVal INT
	IF EXISTS (SELECT 1 FROM msIngredients WITH(NOLOCK) WHERE IngredientID = @IngredientID AND AuditedActivity <> 'D')
	BEGIN
		UPDATE msIngredients
		SET IngredientName = @IngredientName,
			IngredientQuantity = @IngredientQuantity,
			IngredientUnit = @IngredientUnit,
			RecipeID = @RecipeID,
			AuditedActivity = 'U',
			AuditedTime = GETDATE()
		WHERE IngredientID = @IngredientID AND AuditedActivity <> 'D'
		SET @RetVal = @IngredientID
	END
	ELSE
	BEGIN
		INSERT INTO msIngredients
		(RecipeID, IngredientName, IngredientQuantity, IngredientUnit, AuditedActivity, AuditedTime)
		VALUES
		(@RecipeID , @IngredientName, @IngredientQuantity, @IngredientUnit, 'I', GETDATE())
		SET @RetVal = SCOPE_IDENTITY()
	END
	SELECT @RecipeID = @RetVal
END
GO

-- SEEDING msDish
INSERT INTO msDish
VALUES (1,'Makanan', 50000, 'I',GETDATE())

-- SEEDING msRecipe
INSERT INTO msRecipe
VALUES (2, 'Fried Rice', 'Ini nasi goreng', 'I', GETDATE())

-- SEEDING msIngredient
INSERT INTO msIngredients
VALUES (4, 'Rices', 5, 'butir', 'I', GETDATE())

-- Test Procedure Recipe
Exec Recipes_Delete @RecipeIDs = '6'
EXEC Recipes_GetByDishID @DishId = 2
EXEC Recipes_GetByID @RecipeId = '5'
EXEC Recipe_InsertUpdate @RecipeID = 5, @RecipeName = 'Nasi Bakar Komplit', @RecipeDesc = 'Ini nasi bakar 2' , @DishID = 2

--Test Procedure Ingredient
EXEC Ingredients_Delete @IngredientsIDs = '2'
EXEC Ingredients_GetByRecipeID @RecipeId = 3
EXEC Ingredients_GetByID @IngredientId = 3
EXEC Ingredient_InsertUpdate @IngredientID = null, @IngredientName = 'Bakso Bakar', @IngredientQuantity = 20, @IngredientUnit = 'buah', @RecipeId = 1003