using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;
using PersonelTakip_B.Data;
using PersonelTakip_B.Dtos;
using PersonelTakip_B.Models;

namespace PersonelTakip_B
{
    /// <summary>
    /// Personel ve görev işlemleri. Tüm veri işlemleri SQL Server'daki stored procedure'ler
    /// üzerinden EF Core ile yapılır (FromSqlRaw / ExecuteSqlRaw).
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppDBContext _context;
        private int perid = 0;   // Seçili personel (çift tıklama ile seçilir)
        private int gorevid = 0; // Seçili görev

        public MainWindow()
        {
            InitializeComponent();
            _context = new AppDBContext();
            GorevleriGetir();
            PersonelGetir();
        }

        private void PersonelGetir()
        {
            try
            {
                var personeller = _context.personelliste.FromSqlRaw("EXEC spPersonelListele").ToList();
                dgvPersonel.ItemsSource = personeller;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personeller getirilirken bir hata oluştu. " + ex.Message);
            }
        }

        private void GorevleriGetir()
        {
            try
            {
                var gorevler = _context.gorev.FromSqlRaw("EXEC spGorevListele").AsNoTracking().ToList();
                txtCbb.ItemsSource = gorevler;
                dgvGorev.ItemsSource = gorevler;
            }
            catch (Exception e)
            {
                MessageBox.Show("Görevler getirilirken bir hata oluştu. " + e.Message);
            }
        }

        // Stored procedure çalıştırır; SQL hatalarını (ör. RAISERROR) kullanıcıya gösterir
        private bool ProsedurCalistir(string sql, params SqlParameter[] parametreler)
        {
            try
            {
                _context.Database.ExecuteSqlRaw(sql, parametreler);
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Veritabanı Hatası", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        private bool PersonelFormuGecerliMi()
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text))
            {
                MessageBox.Show("Ad ve soyad boş bırakılamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (txtTelefon.Text.Trim().Length > 10)
            {
                MessageBox.Show("Telefon en fazla 10 haneli olabilir (ör. 5551234567).", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (txtCbb.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir görev seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private SqlParameter[] PersonelParametreleri()
        {
            return new[]
            {
                new SqlParameter("@Ad", txtAd.Text.Trim()),
                new SqlParameter("@Soyad", txtSoyad.Text.Trim()),
                new SqlParameter("@Telefon", txtTelefon.Text.Trim()),
                new SqlParameter("@GorevID", (int)txtCbb.SelectedValue)
            };
        }

        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {
            if (!PersonelFormuGecerliMi()) return;
            if (ProsedurCalistir("EXEC spPersonelEkle @Ad, @Soyad, @Telefon, @GorevID", PersonelParametreleri()))
            {
                Temizle();
                PersonelGetir();
            }
        }

        private void btnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (perid == 0)
            {
                MessageBox.Show("Güncellemek için listeden bir personele çift tıklayın.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!PersonelFormuGecerliMi()) return;

            var parametreler = new List<SqlParameter> { new SqlParameter("@PersonelID", perid) };
            parametreler.AddRange(PersonelParametreleri());
            if (ProsedurCalistir("EXEC spPersonelGuncelle @PersonelID, @Ad, @Soyad, @Telefon, @GorevID", parametreler.ToArray()))
            {
                Temizle();
                PersonelGetir();
            }
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (perid == 0)
            {
                MessageBox.Show("Silmek için listeden bir personele çift tıklayın.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show(txtAd.Text + " " + txtSoyad.Text + " silinsin mi?", "Silme Onayı",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            if (ProsedurCalistir("EXEC spPersonelSil @PersonelID", new SqlParameter("@PersonelID", perid)))
            {
                Temizle();
                PersonelGetir();
            }
        }

        private void btnTemizle_Click(object sender, RoutedEventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            perid = 0;
            txtAd.Clear();
            txtSoyad.Clear();
            txtTelefon.Clear();
            txtCbb.SelectedIndex = -1;
        }

        private void btnGorevEkle_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGorevAdi.Text))
            {
                MessageBox.Show("Görev adı boş bırakılamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ProsedurCalistir("EXEC spGorevEkle @GorevAdi", new SqlParameter("@GorevAdi", txtGorevAdi.Text.Trim())))
            {
                gorevid = 0;
                txtGorevAdi.Clear();
                GorevleriGetir();
            }
        }

        private void btnGorevSil_Click(object sender, RoutedEventArgs e)
        {
            if (gorevid == 0)
            {
                MessageBox.Show("Silmek için listeden bir göreve çift tıklayın.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Görevde personel varsa spGorevSil RAISERROR ile hata döndürür ve mesaj gösterilir
            if (ProsedurCalistir("EXEC spGorevSil @GorevID", new SqlParameter("@GorevID", gorevid)))
            {
                gorevid = 0;
                txtGorevAdi.Clear();
                GorevleriGetir();
            }
        }

        // Personel satırına çift tıklanınca bilgileri forma getirir
        private void dgv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgvPersonel.SelectedItem is not PersonelListeDTO secili) return;
            perid = secili.PersonelID;
            txtAd.Text = secili.Ad;
            txtSoyad.Text = secili.Soyad;
            txtTelefon.Text = secili.Telefon?.Trim();
            txtCbb.SelectedValue = secili.GorevID;
        }

        // Görev satırına çift tıklanınca silinecek görev seçilir
        private void dgvGorev_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgvGorev.SelectedItem is not tblGorev secili) return;
            gorevid = secili.GorevID;
            txtGorevAdi.Text = secili.GorevAdi;
        }
    }
}
