CREATE TABLE [dbo].[Proizvodjac]
(
	id              INTEGER NOT NULL IDENTITY(1,1),
    naziv           VARCHAR(30) NOT NULL,
    grad_id         INTEGER NOT NULL,
)
