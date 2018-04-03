ALTER TABLE [dbo].[StavkaFakture]
	ADD CONSTRAINT [StavkaFakture_FK2]
	FOREIGN KEY (stavkaskldok_rednibroj,
    stavkaskldok_skladistenidokument_id,
    stavkaskldok_idproizvod,
    stavkaskldok_idskladista)
	REFERENCES [StavkaSklDokumenta] (rednibroj,
        skladistenidokument_id,
        zalihe_proizvod_id,
        zalihe_idskladista)
