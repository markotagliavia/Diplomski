ALTER TABLE [dbo].[Zalihe]
	ADD CONSTRAINT [Zalihe_FK1]
	FOREIGN KEY (skladiste_id)
	REFERENCES [Skladiste] (id)
