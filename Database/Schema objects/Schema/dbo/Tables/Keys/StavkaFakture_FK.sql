ALTER TABLE [dbo].[StavkaFakture]
	ADD CONSTRAINT [StavkaFakture_FK]
	FOREIGN KEY (faktura_id)
	REFERENCES [Faktura] (id)
