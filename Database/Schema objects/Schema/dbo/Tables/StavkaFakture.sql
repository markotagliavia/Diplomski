﻿CREATE TABLE [dbo].[StavkaFakture]
(
	rednibroj                             INTEGER NOT NULL,
    kolicina                              FLOAT NOT NULL,
    rabat                                 FLOAT,
    cena                                  FLOAT,
	storno							      Bit DEFAULT 0 NULL,
    faktura_id                            INTEGER NOT NULL,
    zalihe_proizvod_id                    INTEGER NOT NULL,
    zalihe_skladiste_id                   INTEGER NOT NULL,
    stavkaprofakture_rednibroj            INTEGER,
    stavkaprofakture_profaktura_id        INTEGER,
    stavkaskldok_rednibroj                INTEGER,
    stavkaskldok_skladistenidokument_id   INTEGER,
    stavkaskldok_idproizvod               INTEGER,
    stavkaskldok_idskladista              INTEGER
)
