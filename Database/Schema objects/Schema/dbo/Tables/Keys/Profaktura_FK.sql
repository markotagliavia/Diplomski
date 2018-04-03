ALTER TABLE [dbo].[Profaktura]
	ADD CONSTRAINT [Profaktura_FK]
	FOREIGN KEY (poslovnipartner_mbr)
	REFERENCES [PoslovniPartner] (mbr)
