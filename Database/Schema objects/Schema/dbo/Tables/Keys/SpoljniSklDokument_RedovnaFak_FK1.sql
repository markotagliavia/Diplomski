ALTER TABLE [dbo].[SpoljniSklDokument_RedovnaFak]
	ADD CONSTRAINT [SpoljniSklDokument_RedovnaFak_FK1]
	FOREIGN KEY (spoljniredovniskldok_id )
	REFERENCES [SkladisteniDokument] (id)
