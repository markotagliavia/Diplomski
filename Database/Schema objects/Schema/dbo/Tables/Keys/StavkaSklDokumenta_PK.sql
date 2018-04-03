ALTER TABLE [dbo].[StavkaSklDokumenta]
	ADD CONSTRAINT [StavkaSklDokumenta_PK]
	PRIMARY KEY (rednibroj, SkladisteniDokument_id, Zalihe_Proizvod_id, Zalihe_idSkladista)
