CREATE TABLE [dbo].[City]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Mjesto] [nvarchar](50) NOT NULL,
	[Zupanija] [nvarchar](50) NOT NULL,
	[Drzava] [nvarchar](50) NULL,
	[Posta] [nvarchar](50) NULL,
	[Prirez] [decimal](8, 2) NOT NULL,
	[Sifra] [nvarchar](50) NULL,
)
