CREATE TABLE [dbo].[Proizvod]
(
	id                INTEGER NOT NULL IDENTITY(1,1),
    naziv             VARCHAR(30) NOT NULL,
    minimumkolicine   FLOAT NOT NULL,
    jedinicamere_id   INTEGER NOT NULL,
    sifra             VARCHAR(30) NOT NULL,
    proizvodjac_id    INTEGER NOT NULL
)
