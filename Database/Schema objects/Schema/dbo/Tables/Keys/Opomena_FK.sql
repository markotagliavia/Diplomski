ALTER TABLE [dbo].[Opomena]
	ADD CONSTRAINT [Opomena_FK]
	FOREIGN KEY (redovnafaktura_id)
	REFERENCES [Faktura] (id)
