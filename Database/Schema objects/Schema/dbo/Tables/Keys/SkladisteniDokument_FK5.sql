ALTER TABLE [dbo].[SkladisteniDokument]
	ADD CONSTRAINT [SkladisteniDokument_FK5]
	FOREIGN KEY (skladiste_id1)
	REFERENCES [Skladiste] (id)
