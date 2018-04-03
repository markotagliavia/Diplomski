ALTER TABLE [dbo].[PoslovniPartner]
	ADD CONSTRAINT [PoslovniPartner_FK]
	FOREIGN KEY (grad_id )
	REFERENCES [Grad] (id)
