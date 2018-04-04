ALTER TABLE [dbo].[Permission_Uloga]
	ADD CONSTRAINT [Permission_Uloga_FK1]
	FOREIGN KEY (permission_id)
	REFERENCES [Permission] (id)
