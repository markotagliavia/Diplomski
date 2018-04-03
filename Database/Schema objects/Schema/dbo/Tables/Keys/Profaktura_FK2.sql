ALTER TABLE [dbo].[Profaktura]
	ADD CONSTRAINT [Profaktura_FK2]
	FOREIGN KEY (zaposleni_id)
	REFERENCES [Zaposleni] (id)
