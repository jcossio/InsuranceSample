CREATE TABLE [dbo].[ClientContract]
(
	[ClientContractId] INT NOT NULL PRIMARY KEY, 
    [ClientId] INT NOT NULL, 
    [PolicyId] INT NOT NULL, 
    [EffectDate] DATETIME NOT NULL, 
    [CoverageMonths] SMALLINT NOT NULL, 
    [MonthlyPremium] MONEY NOT NULL, 
    [RiskTypeId] INT NOT NULL, 
    [Cancelled] BIT NOT NULL
)
