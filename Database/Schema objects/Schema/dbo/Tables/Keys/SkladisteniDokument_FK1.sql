ALTER TABLE [dbo].[SkladisteniDokument]
	ADD CONSTRAINT [SkladisteniDokument_FK1]
	FOREIGN KEY (zaposleniskladista_zaposleni_id,zaposleniskladista_skladiste_id)
	REFERENCES [ZaposleniSkladista] (zaposleni_id,skladiste_id)
