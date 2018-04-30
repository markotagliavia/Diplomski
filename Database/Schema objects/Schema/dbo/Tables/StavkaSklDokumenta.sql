CREATE TABLE [dbo].[StavkaSklDokumenta]
(
	storno                     bit,
    kolicina                   FLOAT NOT NULL,
    rednibroj                  INTEGER NOT NULL,
    zalihe_proizvod_id         INTEGER NOT NULL,
    zalihe_idskladista         INTEGER NOT NULL,
    skladistenidokument_id     INTEGER NOT NULL,
    stavkafakture_faktura_id   INTEGER,
    stavkafakture_rednibroj    INTEGER
)
