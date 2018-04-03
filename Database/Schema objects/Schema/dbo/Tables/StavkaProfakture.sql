CREATE TABLE [dbo].[StavkaProfakture]
(
	kolicina                   FLOAT NOT NULL,
    rabat                      FLOAT NOT NULL,
    cena                       FLOAT NOT NULL,
    rednibroj                  INTEGER NOT NULL,
    profaktura_id              INTEGER NOT NULL,
    zalihe_proizvod_id         INTEGER NOT NULL,
    zalihe_skladiste_id        INTEGER NOT NULL,
    stavkafakture_faktura_id   INTEGER,
    stavkafakture_rednibroj    INTEGER
)
