ALTER TABLE [dbo].[StavkaKompenzacije]
	ADD CONSTRAINT [StavkaKompenzacije_FK2]
	FOREIGN KEY (stavkafakture_faktura_id1,stavkafakture_rednibroj1)
	REFERENCES [StavkaFakture] (faktura_id,rednibroj)
