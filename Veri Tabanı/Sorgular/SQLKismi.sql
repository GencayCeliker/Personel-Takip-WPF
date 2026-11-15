CREATE DATABASE dbPersonelTakip_B
GO
USE dbPersonelTakip_B
GO
CREATE TABLE tblGorev (
	GorevID INT PRIMARY KEY IDENTITY(1,1),
	GorevAdi NVARCHAR(50) 
)
GO 
CREATE TABLE tblPersonel (
	PersonelID INT IDENTITY(1,1) PRIMARY KEY ,
	Ad NVARCHAR(250),
	Soyad NVARCHAR(50),
	Telefon NCHAR(10),
	GorevID INT FOREIGN KEY (GorevID) REFERENCES tblGorev(GorevID)
)
GO
INSERT INTO tblGorev (GorevAdi) VALUES ('Genel Müdür'),('Kısım Amiri'),('Formen'),('Usta Başı'),('İşçi'),('Güvenlik')
GO
CREATE PROCEDURE spGorevListele
AS BEGIN
SET NOCOUNT ON
	SELECT * FROM tblGorev ORDER BY GorevAdi
END
GO
CREATE PROCEDURE spGorevEkle
	@GorevAdi NVARCHAR(50)
AS BEGIN
SET NOCOUNT ON
	INSERT INTO tblGorev (GorevAdi) VALUES (@GorevAdi)
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

CREATE PROCEDURE spPersonelSil
	@PersonelID INT
	AS BEGIN
	SET NOCOUNT ON
	DELETE FROM tblPersonel WHERE PersonelID = @PersonelID
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