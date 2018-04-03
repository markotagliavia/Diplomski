ALTER TABLE [dbo].[Profaktura]
	ADD CONSTRAINT [Profaktura_FK1]
	FOREIGN KEY (redovnafaktura_id)
	REFERENCES [Faktura] (id)
