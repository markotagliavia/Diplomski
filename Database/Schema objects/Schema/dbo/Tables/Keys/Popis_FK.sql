ALTER TABLE [dbo].[Popis]
	ADD CONSTRAINT [Popis_FK]
	FOREIGN KEY (skladiste_id)
	REFERENCES [Skladiste] (id)
