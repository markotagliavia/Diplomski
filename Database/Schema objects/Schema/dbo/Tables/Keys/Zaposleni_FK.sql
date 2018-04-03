ALTER TABLE [dbo].[Zaposleni]
	ADD CONSTRAINT [Zaposleni_FK]
	FOREIGN KEY (grad_id)
	REFERENCES [Grad] (id)
