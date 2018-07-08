CREATE TABLE [dbo].[Notification]
(
	[Id] INT NOT NULL Identity(1,1),
	aplikacija VARCHAR(30) NOT NULL,
	adresa VARCHAR(30) NULL,
	tekst VARCHAR(50) NOT NULL,
	procitana bit NOT NULL,
	obradjena bit NOT NULL,
	idDokumenta int NULL

)
