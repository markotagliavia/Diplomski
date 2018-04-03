ALTER TABLE [dbo].[Faktura]
	ADD CONSTRAINT [Faktura_FK]
	FOREIGN KEY (zaposleni_id)
	REFERENCES [Zaposleni] (id)
