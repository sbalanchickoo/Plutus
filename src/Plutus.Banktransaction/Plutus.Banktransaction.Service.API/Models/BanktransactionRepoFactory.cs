using Microsoft.Extensions.Configuration;
using Plutus.Banktransaction.Service.API.Interfaces;
using Plutus.SharedLibrary.CS.Interfaces;
using System;
using System.IO;

namespace Plutus.Banktransaction.Service.API.Models
{
    public class BanktransactionRepoFactory : IBankTransactionRepo
    {
        public static object ConfigurationManager { get; private set; }

        public static object Repo { get; private set; }

        public IBankTransaction GetRepo()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            string typeName = configuration["RepositoryType"];

            Type repoType = Type.GetType(typeName);
            object repoInstance = Activator.CreateInstance(repoType);
            IBankTransaction repo = repoInstance as IBankTransaction;
            return repo;
        }
        
    }
}

