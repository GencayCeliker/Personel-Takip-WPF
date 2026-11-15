using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PersonelTakip_B.Dtos;
using PersonelTakip_B.Models;

namespace PersonelTakip_B.Data
{
    public class AppDBContext:DbContext //1 KLASI BAŞKA KLASIN KOPYASI OLARAK BU ŞEKLDE YAPIYORUZ
    {
        public DbSet<tblPersonel> personel {get; set;} 
        public DbSet<tblGorev> gorev {get; set;} 
        public DbSet<PersonelListeDTO> personelliste {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;" +
                "Database=dbPersonelTakip_B;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblGorev>().ToTable("tblGorev");
            modelBuilder.Entity<tblPersonel>().ToTable("tblPersonel");
            modelBuilder.Entity<PersonelListeDTO>().HasNoKey();
        }
        
    }
}
