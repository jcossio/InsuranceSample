SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[ClientContract];
GO

CREATE TABLE [dbo].[ClientContract]
(
	[ClientContractId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ClientId] INT NOT NULL, 
    [PolicyId] INT NOT NULL, 
    [EffectDate] DATETIME NOT NULL, 
    [CoverageMonths] SMALLINT NOT NULL, 
    [MonthlyPremium] MONEY NOT NULL, 
    [RiskTypeId] INT NOT NULL, 
    [Cancelled] BIT NOT NULL
);
