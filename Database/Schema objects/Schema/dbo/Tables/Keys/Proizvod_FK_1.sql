ALTER TABLE [dbo].[Proizvod]
	ADD CONSTRAINT [Proizvod_FK1]
	FOREIGN KEY (proizvodjac_id)
	REFERENCES [Proizvodjac] (id)
