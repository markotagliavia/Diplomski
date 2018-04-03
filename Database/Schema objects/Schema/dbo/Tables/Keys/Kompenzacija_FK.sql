ALTER TABLE [dbo].[Kompenzacija]
	ADD CONSTRAINT [Kompenzacija_FK]
	FOREIGN KEY (zaposleni_id)
	REFERENCES [Zaposleni] (id)
