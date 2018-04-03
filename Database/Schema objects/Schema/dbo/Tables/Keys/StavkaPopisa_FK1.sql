ALTER TABLE [dbo].[StavkaPopisa]
	ADD CONSTRAINT [StavkaPopisa_FK1]
	FOREIGN KEY (proizvod_id )
	REFERENCES [Proizvod] (id)
