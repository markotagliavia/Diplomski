CREATE TABLE [dbo].[StavkaPopisa]
(
	rednibroj      INTEGER NOT NULL,
    kolicina       FLOAT NOT NULL,
    proizvod_id    INTEGER NOT NULL,
    popis_id       INTEGER NOT NULL,
    skladiste_id   INTEGER NOT NULL,
    raf            VARCHAR(30)
)
