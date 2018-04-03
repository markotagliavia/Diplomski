CREATE TABLE [dbo].[PoslovniPartner]
(
	mbr            INTEGER NOT NULL IDENTITY(1,1),
    pib            VARCHAR(30),
    naziv          VARCHAR(30) NOT NULL,
    adresa         VARCHAR(30) NOT NULL,
    dugovanja      FLOAT NOT NULL,
    email          VARCHAR(30),
    brojtelefona   VARCHAR(30) NOT NULL,
    tekuciracun    VARCHAR(30) NOT NULL,
    grad_id        INTEGER NOT NULL
)
