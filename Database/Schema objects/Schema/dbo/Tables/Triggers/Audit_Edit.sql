CREATE TRIGGER [Audit_Edit]
ON dbo.Audit
INSTEAD OF UPDATE
AS
BEGIN
 RAISERROR ('Edit not allowed to Audit table (source = instead of)', 16, 1)	
END
