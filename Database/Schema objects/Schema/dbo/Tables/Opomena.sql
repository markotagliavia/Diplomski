CREATE TABLE [dbo].[Opomena]
(
	datum               DATE NOT NULL,
    id                  INTEGER NOT NULL IDENTITY(1,1),
    redovnafaktura_id   INTEGER NOT NULL
)
