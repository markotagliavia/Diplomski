ALTER TABLE [dbo].[Proizvodjac]
	ADD CONSTRAINT [Proizvodjac_FK]
	FOREIGN KEY (grad_id)
	REFERENCES [Grad] (id)
