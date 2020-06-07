using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGames.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> opcije) : base(opcije)
        {
        }
        public DbSet<Igrica> Igrica { get; set; }
        public DbSet<Kategorija> Kategorija { get; set; }
        public DbSet<KategorijaIgrica> KategorijaIgrica { get; set; }
        public DbSet<Novosti> Novosti { get; set; }
        public DbSet<KorisnickiNalog> KorisnickiNalog { get; set; }
        public DbSet<NalogIgrice> NalogIgrice { get; set; }
        public DbSet<SlikaIgrice> SlikaIgrice { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Igrica>().ToTable("Igrica");
            modelBuilder.Entity<Kategorija>().ToTable("Kategorija");
            modelBuilder.Entity<KategorijaIgrica>().ToTable("KategorijaIgrica");
            modelBuilder.Entity<Novosti>().ToTable("Novosti");
            modelBuilder.Entity<KorisnickiNalog>().ToTable("KorisnickiNalog");
            modelBuilder.Entity<NalogIgrice>().ToTable("NalogIgrice");
            modelBuilder.Entity<SlikaIgrice>().ToTable("SlikaIgirce");
        }
    }
}
