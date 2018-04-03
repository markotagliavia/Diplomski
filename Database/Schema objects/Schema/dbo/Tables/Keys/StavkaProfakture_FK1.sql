ALTER TABLE [dbo].[StavkaProfakture]
	ADD CONSTRAINT [StavkaProfakture_FK1]
	FOREIGN KEY (stavkafakture_faktura_id,stavkafakture_rednibroj)
	REFERENCES [StavkaFakture] (faktura_id,rednibroj)
