CREATE TABLE [dbo].[Schedule] (
    [EventId]    INT          IDENTITY (1, 1) NOT NULL,
    [EventDyno]  VARCHAR (25) NOT NULL DEFAULT "RD3 002",
    [EventInfo1] VARCHAR (25) NOT NULL DEFAULT "DO SOMETHING TODAY",
    [EventInfo2] VARCHAR (25) NULL,
    [EventInfo3] VARCHAR (25) NULL,
    [EventMonth] INT NOT NULL DEFAULT 1, 
    [EventDay] INT NOT NULL DEFAULT 1, 
    [EventYear] INT NOT NULL DEFAULT 2023, 
    PRIMARY KEY CLUSTERED ([EventId] ASC)
);

