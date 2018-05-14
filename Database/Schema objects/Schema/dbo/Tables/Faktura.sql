CREATE TABLE [dbo].[Faktura]
(
	datumfakturisanja    DATE NULL,
    upripremi            bit NOT NULL,
    id                   Integer NOT NULL IDENTITY(1,1),
    active               bit NOT NULL,
    zaposleni_id         INTEGER NOT NULL,
    datumprometadobara   DATE NULL,
	redovna				 BIT NOT NULL,
	ulazna				 BIT NOT NULL,
	placeno           FLOAT NULL,
    pdv               FLOAT NULL,
    otpremljena       bit NULL,
    rokplacanja       DATE NULL,
    avansnoplacanje   bit NULL,
    avans             FLOAT NULL,
    profaktura_id     INTEGER NULL,
    likvidirano       bit NULL,
	poslovnipartner_mbr   INTEGER NULL,
	stornoceo			  bit NOT NULL DEFAULT 0,
	oznaka			      VARCHAR(30)
)
