using InsuranceSample.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace InsuranceSample.Infrastructure
{
    public class InsuranceContext: DbContext
    {
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                //optionsBuilder.UseSqlServer("Server=(localdb)\\.;Database=Insurance;Trusted_Connection=True;");
                optionsBuilder.UseSqlite("Data Source=Insurance.db");
        }
    }
}

