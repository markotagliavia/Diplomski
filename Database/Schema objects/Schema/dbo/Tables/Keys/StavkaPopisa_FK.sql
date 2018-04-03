ALTER TABLE [dbo].[StavkaPopisa]
	ADD CONSTRAINT [StavkaPopisa_FK]
	FOREIGN KEY (popis_id)
	REFERENCES [Popis] (id)
