ALTER TABLE [dbo].[StavkaKompenzacije]
	ADD CONSTRAINT [StavkaKompenzacije_FK1]
	FOREIGN KEY (stavkafakture_faktura_id,
    stavkafakture_rednibroj,stavkafakture_faktura_id1,stavkafakture_rednibroj1)
	REFERENCES [StavkaFakture] (faktura_id,rednibroj,faktura_id,rednibroj)
