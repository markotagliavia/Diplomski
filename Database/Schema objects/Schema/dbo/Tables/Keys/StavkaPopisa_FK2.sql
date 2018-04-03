ALTER TABLE [dbo].[StavkaPopisa]
	ADD CONSTRAINT [StavkaPopisa_FK2]
	FOREIGN KEY (skladiste_id)
	REFERENCES [Skladiste] (id)
