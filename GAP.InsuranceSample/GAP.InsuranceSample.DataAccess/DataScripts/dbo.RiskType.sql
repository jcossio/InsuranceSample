﻿SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[RiskType];


GO
CREATE TABLE [dbo].[RiskType]
(
	[RiskTypeId] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL
);
