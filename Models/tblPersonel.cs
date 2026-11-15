using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace PersonelTakip_B.Models
{
   public class tblPersonel
    {
        [Key]
        public int PersonelID { get; set; }
        public string Ad { get; set; }=string.Empty;
        public string Soyad { get; set; }=string.Empty;
        public string Telefon { get; set; }=string.Empty;
        public int GorevID { get; set; }
    }
}
