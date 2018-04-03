ALTER TABLE [dbo].[StavkaSklDokumenta]
	ADD CONSTRAINT [StavkaSklDokumenta_FK]
	FOREIGN KEY (skladistenidokument_id)
	REFERENCES [SkladisteniDokument] (id)
