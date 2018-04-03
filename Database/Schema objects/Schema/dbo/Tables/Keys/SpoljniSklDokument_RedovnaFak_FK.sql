ALTER TABLE [dbo].[SpoljniSklDokument_RedovnaFak]
	ADD CONSTRAINT [SpoljniSklDokument_RedovnaFak_FK]
	FOREIGN KEY (redovnafaktura_id)
	REFERENCES [Faktura] (id)
