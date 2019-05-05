CREATE TABLE [dbo].[Policy]
(
	[PolicyId] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [MonthlyPremium] MONEY NOT NULL, 
    [RiskTypeId] INT NOT NULL, 
    [Deleted] BIT NOT NULL
)
