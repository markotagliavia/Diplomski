ALTER TABLE [dbo].[StavkaFakture]
	ADD CONSTRAINT [StavkaFakture_FK1]
	FOREIGN KEY (stavkaprofakture_rednibroj,stavkaprofakture_profaktura_id)
	REFERENCES [StavkaProfakture] (rednibroj,profaktura_id)
