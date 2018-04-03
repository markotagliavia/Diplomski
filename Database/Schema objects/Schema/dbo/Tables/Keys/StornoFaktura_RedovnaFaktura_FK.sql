ALTER TABLE [dbo].[StornoFaktura_RedovnaFaktura]
	ADD CONSTRAINT [StornoFaktura_RedovnaFaktura_FK]
	FOREIGN KEY (redovnafaktura_id)
	REFERENCES [Faktura] (id)
