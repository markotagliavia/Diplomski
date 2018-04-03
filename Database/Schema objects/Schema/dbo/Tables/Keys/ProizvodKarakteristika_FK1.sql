ALTER TABLE [dbo].[ProizvodKarakteristika]
	ADD CONSTRAINT [ProizvodKarakteristika_FK1]
	FOREIGN KEY (proizvod_id)
	REFERENCES [Proizvod] (id)
