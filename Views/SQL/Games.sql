CREATE TABLE [dbo].[Games] (
    [gameId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserId]   NCHAR (10)     NULL,
    [time]     NVARCHAR (50)  NULL,
    [date]     NVARCHAR (50)  NULL,
    [gameData] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([gameId] ASC)
);

