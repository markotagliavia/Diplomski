ALTER TABLE [dbo].[Korisnik]
	ADD CONSTRAINT [Korisnik_FK]
	FOREIGN KEY (zaposleni_id)
	REFERENCES [Zaposleni] (id)
