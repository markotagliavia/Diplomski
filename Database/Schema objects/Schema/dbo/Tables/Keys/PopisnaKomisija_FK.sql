ALTER TABLE [dbo].[PopisnaKomisija]
	ADD CONSTRAINT [PopisnaKomisija_FK]
	FOREIGN KEY (popis_id)
	REFERENCES [Popis] (id)
