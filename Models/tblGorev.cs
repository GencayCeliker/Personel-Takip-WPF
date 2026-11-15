using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PersonelTakip_B.Models
{
    public class tblGorev
    {
        [Key] 
        public int GorevID { get; set; }
        public string GorevAdi { get; set; }=string.Empty;
    }
}
