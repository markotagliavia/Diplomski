CREATE TABLE [dbo].[StavkaKompenzacije]
(
	stavkafakture_faktura_id   INTEGER NOT NULL,
    stavkafakture_rednibroj    INTEGER NOT NULL,
	stavkafakture_faktura_id1   INTEGER NOT NULL,
	stavkafakture_rednibroj1    INTEGER NOT NULL,
    kompenzacija_id            INTEGER NOT NULL,
    kolicina                   INTEGER NOT NULL,
    rednibroj                  INTEGER NOT NULL
)
