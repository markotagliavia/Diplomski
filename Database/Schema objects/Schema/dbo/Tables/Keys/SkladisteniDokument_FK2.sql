ALTER TABLE [dbo].[SkladisteniDokument]
	ADD CONSTRAINT [SkladisteniDokument_FK2]
	FOREIGN KEY (poslovnipartner_mbr)
	REFERENCES [PoslovniPartner] (mbr)
