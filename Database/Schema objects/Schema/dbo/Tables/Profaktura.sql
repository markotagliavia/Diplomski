CREATE TABLE [dbo].[Profaktura]
(
	active              bit NOT NULL,
    id                  INTEGER NOT NULL IDENTITY(1,1),
    redovnafaktura_id   INTEGER NOT NULL,
	oznaka		        VARCHAR(30) NOT NULL,		  
    zaposleni_id        INTEGER NOT NULL,
	poslovnipartner_mbr   INTEGER NOT NULL
)
