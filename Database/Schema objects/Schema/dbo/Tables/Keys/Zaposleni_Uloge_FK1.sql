ALTER TABLE [dbo].[Zaposleni_Uloge]
	ADD CONSTRAINT [Zaposleni_Uloge_FK1]
	FOREIGN KEY (zaposleni_id)
	REFERENCES [Zaposleni] (id)
