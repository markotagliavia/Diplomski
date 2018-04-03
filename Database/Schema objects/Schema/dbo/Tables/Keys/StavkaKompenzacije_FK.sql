ALTER TABLE [dbo].[StavkaKompenzacije]
	ADD CONSTRAINT [StavkaKompenzacije_FK]
	FOREIGN KEY (kompenzacija_id)
	REFERENCES [Kompenzacija] (id)
