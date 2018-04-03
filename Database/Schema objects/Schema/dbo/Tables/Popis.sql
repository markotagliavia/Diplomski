CREATE TABLE [dbo].[Popis]
(
	id             INTEGER NOT NULL IDENTITY(1,1),
    datum          DATE NOT NULL,
    skladiste_id   INTEGER NOT NULL,
	oznaka		   VARCHAR(30) NOT NULL
)
