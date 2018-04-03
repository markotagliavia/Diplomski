ALTER TABLE [dbo].[ProizvodKarakteristika]
	ADD CONSTRAINT [ProizvodKarakteristika_FK]
	FOREIGN KEY (karakteristika_id )
	REFERENCES [Karakteristika] (id)
