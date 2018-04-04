CREATE TRIGGER [Audit_Delete]
ON dbo.Audit
INSTEAD OF DELETE
AS
BEGIN
 RAISERROR ('Delete not allowed from Audit table (source = instead of)', 16, 1)	
END
