ALTER TABLE [dbo].[StavkaKompenzacije]
	ADD CONSTRAINT [StavkaKompenzacije_PK]
	PRIMARY KEY (StavkaFakture_Faktura_id, StavkaFakture_redniBroj, Kompenzacija_id)
