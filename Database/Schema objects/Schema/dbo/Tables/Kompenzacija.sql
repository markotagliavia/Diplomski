CREATE TABLE [dbo].[Kompenzacija]
(
	datum          DATE NOT NULL,
    id             INTEGER NOT NULL IDENTITY(1,1),
    active         bit NOT NULL,
    zaposleni_id   INTEGER NOT NULL
)
