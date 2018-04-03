ALTER TABLE [dbo].[PopisnaKomisija]
	ADD CONSTRAINT [PopisnaKomisija_FK1]
	FOREIGN KEY (zaposleni_id)
	REFERENCES [Zaposleni] (id)
