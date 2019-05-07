SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[ClientContractCover];
GO

CREATE TABLE [dbo].[ClientContractCover]
(
	[ClientContractCoverId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ClientContractId] INT NOT NULL, 
    [CoverId] INT NOT NULL, 
    [Percentage] FLOAT NOT NULL
);
