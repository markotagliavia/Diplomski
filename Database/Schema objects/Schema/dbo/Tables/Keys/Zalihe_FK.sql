ALTER TABLE [dbo].[Zalihe]
	ADD CONSTRAINT [Zalihe_FK]
	FOREIGN KEY (proizvod_id)
	REFERENCES [Proizvod] (id)
