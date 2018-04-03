ALTER TABLE [dbo].[ZaposleniSkladista]
	ADD CONSTRAINT [ZaposleniSkladista_FK]
	FOREIGN KEY (skladiste_id)
	REFERENCES [Skladiste] (id)
