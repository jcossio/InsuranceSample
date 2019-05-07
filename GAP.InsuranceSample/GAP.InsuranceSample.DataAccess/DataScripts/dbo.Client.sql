SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Client];


GO
CREATE TABLE [dbo].[Client] (
    [ClientId] INT            IDENTITY(1,1) NOT NULL  PRIMARY KEY, 
    [SSN]      NCHAR (10)     NOT NULL,
    [Name]     NVARCHAR (100) NOT NULL
);


