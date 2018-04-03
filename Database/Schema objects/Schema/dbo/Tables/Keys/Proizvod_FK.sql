ALTER TABLE [dbo].[Proizvod]
	ADD CONSTRAINT [Proizvod_FK]
	FOREIGN KEY (jedinicamere_id )
	REFERENCES [JedinicaMere] (id)
