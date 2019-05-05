CREATE TABLE [dbo].[ClientContractCover]
(
	[ClientContractCoverId] INT NOT NULL PRIMARY KEY, 
    [ClientContractId] INT NOT NULL, 
    [CoverId] INT NOT NULL, 
    [Percentage] FLOAT NOT NULL
)
