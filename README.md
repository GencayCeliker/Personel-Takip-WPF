# 👥 Personel Takip Sistemi (WPF)

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=dotnet&logoColor=white)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![WPF](https://img.shields.io/badge/UI-WPF-0078D7?style=for-the-badge)
![EF Core](https://img.shields.io/badge/EF%20Core-10-6C1D5F?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Stored%20Procedure-CC2927?style=for-the-badge)

Personel ve görev tanımlarını yöneten **WPF / .NET 10** uygulaması. Tüm veri işlemleri SQL Server'daki **stored procedure**'ler ile yapılır; uygulama bunları **EF Core** üzerinden çağırır (`FromSqlRaw` / `ExecuteSqlRaw`).

> Veri Tabanı dersi kapsamında hazırlanmıştır.

---

## 🚀 Özellikler

- **Personel:** Ekleme, güncelleme, silme (onaylı), listeleme. Listede bir satıra çift tıklayınca bilgiler forma gelir.
- **Görev tanımlama:** Görev ekleme ve silme. Görevde çalışan personel varsa silme işlemi veritabanı tarafında `RAISERROR` ile engellenir ve mesaj kullanıcıya gösterilir.
- **Doğrulama:** Ad/soyad zorunlu, telefon en fazla 10 hane, görev seçimi zorunlu
- İki tabloyu birleştiren liste için **DTO** (`PersonelListeDTO`) ve anahtarsız EF Core varlığı (`HasNoKey`)

## 🗄️ Veritabanı (`dbPersonelTakip_B`)

| Nesne | Açıklama |
| --- | --- |
| `tblGorev`, `tblPersonel` | Görevler ve personel (yabancı anahtar ile bağlı) |
| `spPersonelListele` | Personel + görev adı (`JOIN`) |
| `spPersonelEkle`, `spPersonelGuncelle`, `spPersonelSil` | Personel işlemleri |
| `spGorevListele`, `spGorevEkle`, `spGorevSil` | Görev işlemleri (`spGorevSil` içinde `IF EXISTS` + `RAISERROR`) |

## 📂 Proje Yapısı

```
Personel-Takip-WPF/
├── MainWindow.xaml(.cs)   ← Arayüz ve işlemler
├── Data/AppDBContext.cs   ← EF Core DbContext
├── Models/                ← tblPersonel, tblGorev
├── Dtos/                  ← PersonelListeDTO
└── Veri Tabanı/
    ├── dbPersonelTakip_B.sql   ← Kurulum script'i
    └── Sorgular/SQLKismi.sql   ← Ders sırasında yazılan orijinal SQL
```

---

## ⚙️ Kurulum

**Gereksinimler:** Visual Studio 2026 (veya .NET 10 SDK destekleyen Visual Studio 2022) – *.NET masaüstü geliştirme* · SQL Server (Express yeterli) · SQL Server Management Studio (SSMS) 22

1. **İndirin**
   ```bash
   git clone https://github.com/GencayCeliker/Personel-Takip-WPF.git
   ```
2. **Veritabanını kurun (SSMS 22):** `Veri Tabanı/dbPersonelTakip_B.sql` dosyasını açıp **Execute (F5)**'e basın.
3. **Bağlantıyı kontrol edin:** `Data/AppDBContext.cs` içinde `Server=.\SQLEXPRESS;Database=dbPersonelTakip_B` kullanılır.
4. **Çalıştırın:** `PersonelTakip_B.slnx` dosyasını açın ve **F5** ile başlatın.

## 📄 Lisans

Bu proje [MIT](LICENSE) lisansı altında sunulmuştur.
