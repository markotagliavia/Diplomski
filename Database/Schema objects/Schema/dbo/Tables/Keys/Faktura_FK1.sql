ALTER TABLE [dbo].[Faktura]
	ADD CONSTRAINT [Faktura_FK1]
	FOREIGN KEY (poslovnipartner_mbr)
	REFERENCES [PoslovniPartner] (mbr)
