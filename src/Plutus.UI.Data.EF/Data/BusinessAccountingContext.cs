﻿using Microsoft.EntityFrameworkCore;
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

        public DbSet<Client> Clients { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<MasterMerchant> MasterMerchants { get; set; }

        public DbSet<MasterMerchantCategory> MasterMerchantCategories { get; set; }

        public DbSet<Merchant> Merchants { get; set; }

        public DbSet<Salary> Salaries { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionType> TransactionCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");

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
