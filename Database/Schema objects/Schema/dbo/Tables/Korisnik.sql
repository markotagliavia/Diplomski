CREATE TABLE [dbo].[Korisnik]
(
	korisnickoime   VARCHAR(30) NOT NULL,
    lozinka         VARCHAR(65) NOT NULL,
    id              INTEGER NOT NULL IDENTITY(1,1),
    zaposleni_id    INTEGER NOT NULL,
    active          bit NOT NULL,
    ulogovan        bit NOT NULL
)
