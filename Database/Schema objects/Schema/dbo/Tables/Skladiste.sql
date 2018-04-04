CREATE TABLE [dbo].[Skladiste]
(
	id        INTEGER NOT NULL IDENTITY(1,1),
    naziv     VARCHAR(30) NOT NULL,
    adresa    VARCHAR(30) NOT NULL,
    sifra     VARCHAR(30) NOT NULL,
    grad_id   INTEGER NOT NULL,
	active	BIT NOT NULL
)
