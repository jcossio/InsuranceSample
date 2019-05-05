CREATE TABLE [dbo].[PolicyCover]
(
	[PolicyCoverId] INT NOT NULL PRIMARY KEY, 
    [PolicyId] INT NOT NULL, 
    [CoverId] INT NOT NULL, 
    [Percentage] FLOAT NOT NULL
)
