ALTER TABLE [dbo].[ZaposleniSkladista]
	ADD CONSTRAINT [ZaposleniSkladista_FK1]
	FOREIGN KEY (zaposleni_id)
	REFERENCES [Zaposleni] (id)
