CREATE TABLE [dbo].[City]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Mjesto] [nvarchar](100) NOT NULL,
	[Zupanija] [nvarchar](75) NOT NULL,
	[Drzava] [nvarchar](75) NULL,
	[Posta] [nvarchar](10) NULL,
	[Prirez] [decimal](8, 2) NULL,
	[Sifra] [nvarchar](10) NULL, 
    [Porez1] DECIMAL(8, 2) NULL, 
    [Porez2] DECIMAL(8, 2) NULL,
)
