ALTER TABLE [dbo].[StavkaKompenzacije]
	ADD CONSTRAINT [StavkaKompenzacije_FK1]
	FOREIGN KEY (stavkafakture_faktura_id,
    stavkafakture_rednibroj)
	REFERENCES [StavkaFakture] (faktura_id,rednibroj)
