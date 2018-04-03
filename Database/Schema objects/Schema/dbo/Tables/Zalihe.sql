CREATE TABLE [dbo].[Zalihe]
(
	kolicina          FLOAT NOT NULL,
    minimumkolicine   FLOAT NOT NULL,
    proizvod_id       INTEGER NOT NULL,
    skladiste_id      INTEGER NOT NULL,
    raf               VARCHAR(30),
    rezervisano       FLOAT NOT NULL
)
