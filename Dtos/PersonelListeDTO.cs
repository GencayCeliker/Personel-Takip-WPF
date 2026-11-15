using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace PersonelTakip_B.Dtos
{
    [Keyless]
    public class PersonelListeDTO //veri tabanını nasıl buluşturucaz tek tablo olarak 2 tabloyu tek tablo yaptık sorgular kolay olsun diye
    {
        public int PersonelID { get; set; }
        public string? Ad { get; set; }//soru  işaretide boşmu olaraj soruyor  = string.Empty; aynı 
        public string? Soyad { get; set; }
        public string? Telefon { get; set; }
        public int GorevID { get; set; }
        public string? GorevAdi { get; set; }
    }
}
