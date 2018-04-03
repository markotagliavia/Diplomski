CREATE TABLE [dbo].[Zaposleni]
(
	id             INTEGER NOT NULL IDENTITY(1,1),
    jmbg           VARCHAR(30),
    ime            VARCHAR(30) NOT NULL,
    prezime        VARCHAR(30) NOT NULL,
    plata          FLOAT NOT NULL,
    doprinosi      FLOAT NOT NULL,
    bonus          FLOAT NOT NULL,
    adresa         VARCHAR(30) NOT NULL,
    email          VARCHAR(30) NOT NULL,
    tekuciracun    VARCHAR(30) NOT NULL,
    brojtelefona   VARCHAR(30) NOT NULL,
    sef_id   INTEGER,
    grad_id        INTEGER NOT NULL,
    active         bit NOT NULL
)
