ALTER TABLE [dbo].[StornoFaktura_RedovnaFaktura]
	ADD CONSTRAINT [StornoFaktura_RedovnaFaktura_FK1]
	FOREIGN KEY (stornafaktura_id)
	REFERENCES [Faktura] (id)
