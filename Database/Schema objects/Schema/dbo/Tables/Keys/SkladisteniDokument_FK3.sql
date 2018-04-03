ALTER TABLE [dbo].[SkladisteniDokument]
	ADD CONSTRAINT [SkladisteniDokument_FK3]
	FOREIGN KEY (redovniskldok_id)
	REFERENCES [SkladisteniDokument] (id)
