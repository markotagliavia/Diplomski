CREATE TABLE [dbo].[Audit]
(
	id integer NOT NULL IDENTITY(1,1),
	korisnickoime VARCHAR(30) NOT NULL,
	akcija VARCHAR(75) NOT NULL,
	vreme DATETIME NOT NULL,
	tip VARCHAR(10) NOT NULL
)
