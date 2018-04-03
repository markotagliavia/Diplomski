ALTER TABLE [dbo].[Zaposleni]
	ADD CONSTRAINT [Zaposleni_FK2]
	FOREIGN KEY (sef_id)
	REFERENCES [Zaposleni] (id)
