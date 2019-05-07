SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[PolicyCover];


GO
CREATE TABLE [dbo].[PolicyCover]
(
	[PolicyCoverId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [PolicyId] INT NOT NULL, 
    [CoverId] INT NOT NULL, 
    [Percentage] FLOAT NOT NULL
);
