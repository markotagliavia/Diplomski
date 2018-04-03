ALTER TABLE [dbo].[StavkaProfakture]
	ADD CONSTRAINT [StavkaProfakture_FK2]
	FOREIGN KEY (zalihe_proizvod_id,zalihe_skladiste_id)
	REFERENCES [Zalihe] (proizvod_id,skladiste_id)
