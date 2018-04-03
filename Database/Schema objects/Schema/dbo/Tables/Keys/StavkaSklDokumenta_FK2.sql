ALTER TABLE [dbo].[StavkaSklDokumenta]
	ADD CONSTRAINT [StavkaSklDokumenta_FK2]
	FOREIGN KEY (zalihe_proizvod_id,zalihe_idskladista)
	REFERENCES [Zalihe] (proizvod_id,skladiste_id)
