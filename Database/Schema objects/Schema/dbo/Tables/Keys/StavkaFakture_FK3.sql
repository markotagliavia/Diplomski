ALTER TABLE [dbo].[StavkaFakture]
	ADD CONSTRAINT [StavkaFakture_FK3]
	FOREIGN KEY (zalihe_proizvod_id,
    zalihe_skladiste_id)
	REFERENCES [Zalihe] (proizvod_id,
        skladiste_id)
