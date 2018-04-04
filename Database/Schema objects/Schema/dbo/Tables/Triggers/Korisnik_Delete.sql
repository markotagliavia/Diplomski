CREATE TRIGGER [Korisnik_Delete]
ON dbo.Korisnik
INSTEAD OF DELETE
AS
BEGIN

DECLARE @id_brisanog as Integer;
DECLARE @id_zaposlenog_brisanog as Integer;
SET @id_brisanog = -1
SET @id_zaposlenog_brisanog = -1

IF EXISTS(SELECT * FROM DELETED)
    BEGIN
        SET @id_brisanog = (SELECT id from DELETED)
		SET @id_zaposlenog_brisanog = (SELECT zaposleni_id from DELETED)
    END

IF(@id_brisanog != -1)
	BEGIN
		UPDATE dbo.Korisnik
		SET active = 0
		WHERE id = @id_brisanog;

		UPDATE dbo.Zaposleni
		SET active = 0
		WHERE id = @id_zaposlenog_brisanog
	END
END
