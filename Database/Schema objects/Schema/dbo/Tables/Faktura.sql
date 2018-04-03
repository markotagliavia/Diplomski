CREATE TABLE [dbo].[Faktura]
(
	datumfakturisanja    DATE NOT NULL,
    upripremi            bit NOT NULL,
    id                   Integer NOT NULL IDENTITY(1,1),
    active               bit NOT NULL,
    zaposleni_id         INTEGER NOT NULL,
    datumprometadobara   DATE,
	redovna				 BIT NOT NULL,
	ulazna				 BIT,
	placeno           FLOAT NOT NULL,
    pdv               FLOAT NOT NULL,
    otpremljena       bit NOT NULL,
    rokplacanja       DATE NOT NULL,
    avansnoplacanje   bit NOT NULL,
    avans             FLOAT NOT NULL,
    profaktura_id     INTEGER NOT NULL,
    likvidirano       bit NOT NULL,
	poslovnipartner_mbr   INTEGER NOT NULL,
	stornoceo			  bit NOT NULL,
	oznaka			      VARCHAR(30)
)
