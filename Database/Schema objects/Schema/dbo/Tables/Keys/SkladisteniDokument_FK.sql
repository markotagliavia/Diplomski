ALTER TABLE [dbo].[SkladisteniDokument]
	ADD CONSTRAINT [SkladisteniDokument_FK]
	FOREIGN KEY (skladiste_id)
	REFERENCES [Skladiste] (id)
