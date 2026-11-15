/* dbPersonelTakip_B veritabanı - SQL Server'dan dışa aktarıldı (2026-11-15)
   SSMS ile açıp Execute (F5) ile çalıştırın. */
IF DB_ID(N'dbPersonelTakip_B') IS NULL CREATE DATABASE [dbPersonelTakip_B];
GO
USE [dbPersonelTakip_B];
GO

CREATE TABLE [dbo].[tblGorev] (
    [GorevID] int IDENTITY(1,1) NOT NULL,
    [GorevAdi] nvarchar(50) NULL,
    CONSTRAINT [PK__tblGorev__2BC5994910749936] PRIMARY KEY ([GorevID])
);
GO

CREATE TABLE [dbo].[tblPersonel] (
    [PersonelID] int IDENTITY(1,1) NOT NULL,
    [Ad] nvarchar(250) NULL,
    [Soyad] nvarchar(50) NULL,
    [Telefon] nchar(10) NULL,
    [GorevID] int NULL,
    CONSTRAINT [PK__tblPerso__0F0C5751E8A5ECA5] PRIMARY KEY ([PersonelID])
);
GO

SET IDENTITY_INSERT [dbo].[tblGorev] ON;
INSERT INTO [dbo].[tblGorev] ([GorevID], [GorevAdi]) VALUES (1, N'Genel Müdür');
INSERT INTO [dbo].[tblGorev] ([GorevID], [GorevAdi]) VALUES (2, N'Kısım Amiri');
INSERT INTO [dbo].[tblGorev] ([GorevID], [GorevAdi]) VALUES (3, N'Formen');
INSERT INTO [dbo].[tblGorev] ([GorevID], [GorevAdi]) VALUES (4, N'Usta Başı');
INSERT INTO [dbo].[tblGorev] ([GorevID], [GorevAdi]) VALUES (5, N'İşçi');
INSERT INTO [dbo].[tblGorev] ([GorevID], [GorevAdi]) VALUES (6, N'Güvenlik');
SET IDENTITY_INSERT [dbo].[tblGorev] OFF;
GO

ALTER TABLE [dbo].[tblPersonel] ADD CONSTRAINT [FK__tblPerson__Gorev__4CA06362] FOREIGN KEY ([GorevID]) REFERENCES [dbo].[tblGorev] ([GorevID]);
GO

CREATE PROCEDURE spGorevListele
AS BEGIN
SET NOCOUNT ON
	SELECT * FROM tblGorev ORDER BY GorevAdi
END
GO

CREATE PROCEDURE spGorevSil
	@GorevID INT
AS BEGIN
SET NOCOUNT ON
IF EXISTS (SELECT 1 FROM tblPersonel WHERE GorevID = @GorevID)
    BEGIN
		RAISERROR('Bu görevde personel bulunduğu için silinemez.', 16, 1)--16: Hata Seviyesi, 1: Durum 
		RETURN
	END
DELETE FROM tblGorev WHERE GorevID = @GorevID
END
GO

CREATE PROCEDURE spPersonelEkle
	@Ad NVARCHAR(250),
	@Soyad NVARCHAR(50),
	@Telefon NCHAR(10),
	@GorevID INT
	AS BEGIN
	SET NOCOUNT ON
	INSERT INTO tblPersonel VALUES (@Ad, @Soyad, @Telefon, @GorevID)
	END
GO

CREATE PROCEDURE spPersonelGuncelle
	@PersonelID INT,
	@Ad NVARCHAR(250),
	@Soyad NVARCHAR(50),
	@Telefon NCHAR(10),
	@GorevID INT
	AS BEGIN
	SET NOCOUNT ON
	UPDATE tblPersonel SET Ad = @Ad, Soyad = @Soyad, Telefon = @Telefon, GorevID = @GorevID WHERE PersonelID = @PersonelID
	END
GO

CREATE PROCEDURE spPersonelListele
AS BEGIN
SET NOCOUNT ON
SELECT 
	p.PersonelID,
	p.Ad,
	p.Soyad,
	p.Telefon,
	g.GorevID,
	g.GorevAdi FROM tblPersonel p JOIN tblGorev g 
	 ON p.GorevID = g.GorevID ORDER BY p.Ad, p.Soyad
END
GO

CREATE PROCEDURE spPersonelSil
	@PersonelID INT
	AS BEGIN
	SET NOCOUNT ON
	DELETE FROM tblPersonel WHERE PersonelID = @PersonelID
	END
GO

CREATE PROCEDURE spGorevEkle
	@GorevAdi NVARCHAR(50)
AS BEGIN
SET NOCOUNT ON
	INSERT INTO tblGorev (GorevAdi) VALUES (@GorevAdi)
END
GO
