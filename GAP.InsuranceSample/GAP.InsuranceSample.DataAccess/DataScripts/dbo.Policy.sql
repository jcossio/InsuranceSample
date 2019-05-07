SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Policy];


GO
CREATE TABLE [dbo].[Policy]
(
	[PolicyId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [MonthlyPremium] MONEY NOT NULL, 
    [RiskTypeId] INT NOT NULL, 
    [Deleted] BIT NOT NULL
);
