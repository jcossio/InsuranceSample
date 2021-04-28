using InsuranceSample.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace InsuranceSample.Infrastructure
{
    public class InsuranceContext: DbContext
    {
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\.;Database=Insurance;user=ityp;password=ityp;");
        }
    }
}
