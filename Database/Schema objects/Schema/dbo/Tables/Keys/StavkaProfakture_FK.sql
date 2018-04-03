ALTER TABLE [dbo].[StavkaProfakture]
	ADD CONSTRAINT [StavkaProfakture_FK]
	FOREIGN KEY (profaktura_id)
	REFERENCES [Profaktura] (id)
