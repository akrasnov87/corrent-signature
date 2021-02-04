using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Signature
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Signature.Models.Signature> Signatures { get; set; }

        public DbSet<Signature.Models.Street> Streets { get; set; }
        public DbSet<Signature.Models.House> HousesOrigins { get; set; }
        public DbSet<Signature.Models.Appartament> Appartaments { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=dev-ws-v-07;Port=5432;Database=vote-dev;Username=mobnius;Password=mobnius-0");
        }
    }
}
