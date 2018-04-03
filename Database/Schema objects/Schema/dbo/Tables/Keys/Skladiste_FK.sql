ALTER TABLE [dbo].[Skladiste]
	ADD CONSTRAINT [Skladiste_FK]
	FOREIGN KEY (grad_id)
	REFERENCES [Grad] (id)
