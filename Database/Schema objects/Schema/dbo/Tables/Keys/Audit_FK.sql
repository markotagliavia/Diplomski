ALTER TABLE [dbo].[Audit]
	ADD CONSTRAINT [Audit_FK]
	FOREIGN KEY (korisnickoime)
	REFERENCES [Korisnik] (korisnickoime)
	ON UPDATE CASCADE;
