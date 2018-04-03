CREATE TABLE [dbo].[SkladisteniDokument]
(
	id                                INTEGER NOT NULL IDENTITY(1,1),
    datum                             DATE NOT NULL,
    upripremi                         bit NOT NULL,
    zaposleniskladista_zaposleni_id   INTEGER NOT NULL,
    zaposleniskladista_skladiste_id   INTEGER NOT NULL,
    sifra                             VARCHAR(30) NOT NULL,
    active                            bit NOT NULL,
	redovni							  bit NOT NULL,
	tipredovnog						  VARCHAR(10) NOT NULL,
	redovniskldok_id				  INTEGER NOT NULL,
    storniranceo					  bit NOT NULL,
	vozac							  VARCHAR(30) NOT NULL,
    regbr                             VARCHAR(30),
    nacinotpreme                      VARCHAR(40),
    izdao                             VARCHAR(30) NOT NULL,
    primio                            VARCHAR(30) NOT NULL,
    poslovnipartner_mbr               INTEGER NOT NULL,
	skladiste_id					  INTEGER NOT NULL,


)
