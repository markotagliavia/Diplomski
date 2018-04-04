ALTER TABLE [dbo].[Permission_Uloga]
	ADD CONSTRAINT [Permission_Uloga_FK]
	FOREIGN KEY (uloga_id)
	REFERENCES [Uloga] (id)
