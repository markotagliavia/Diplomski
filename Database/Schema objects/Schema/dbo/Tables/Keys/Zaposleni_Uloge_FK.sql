ALTER TABLE [dbo].[Zaposleni_Uloge]
	ADD CONSTRAINT [Zaposleni_Uloge_FK]
	FOREIGN KEY (uloga_id)
	REFERENCES [Uloga] (id)
