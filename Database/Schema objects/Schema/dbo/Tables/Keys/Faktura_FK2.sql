ALTER TABLE [dbo].[Faktura]
	ADD CONSTRAINT [Faktura_FK2]
	FOREIGN KEY (profaktura_id)
	REFERENCES [Profaktura] (id)
