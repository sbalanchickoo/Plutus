using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Plutus.UI.Shared.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Plutus.UI.Data.EF.Data
{
    public class BusinessAccountingContext : DbContext
    {
        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                if(_connectionString == null)
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + "appsettings.json"))
                    {
                        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
                        var configuration = builder.Build();
                        _connectionString = configuration["Connection:ConnectionString"];
                    }
                }
                return _connectionString;
            }
            set { _connectionString = value; }
        }

        public DbSet<Client> Client { get; set; }

        public DbSet<Invoice> Invoice { get; set; }

        public DbSet<MasterMerchant> MasterMerchant { get; set; }

        public DbSet<MasterMerchantCategory> MasterMerchantCategory { get; set; }

        public DbSet<Merchant> Merchant { get; set; }

        public DbSet<Salary> Salary { get; set; }

        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<TransactionType> TransactionCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType() { TransactionTypeName = "Incorporation Fee" }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlite(ConnectionString);
        }
    }
}
