using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;

namespace Plutus.Banktransaction.Service.API.Controllers.V1
{
    /// <summary>
    /// Central controller for bank transactions
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [FormatFilter]
    public class BanktransactionsController : Controller
    {
        IBankTransaction _repo;
        private readonly ILogger<BanktransactionsController> _logger;

        /// <summary>
        /// Primary constructor with DI for Logging and Repository
        /// </summary>
        public BanktransactionsController(IBankTransaction repo, ILogger<BanktransactionsController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogInformation(200, "BanktransactionController invoked");
        }

        /// <summary>
        /// Returns details about source repository
        /// </summary>
        [HttpGet]
        [Route("[action]/{format?}")]
        public IActionResult SourceDetail()
        {
            InputDataSource ds = _repo.GetSourceDetails();
            _logger.LogInformation(200, "SourceDetail request complete");
            return Ok(ds);
        }

        /// <summary>
        /// Primary action, returns Bank transactions
        /// </summary>
        [HttpGet]
        [Route("[action]/{format?}")]
        [Route("")]
        public IActionResult BankTransactions()
        {
            List<BankTransaction> txns = new List<BankTransaction>();
            txns = _repo.GetBankTransactions().ToList();
            var result = txns;
            _logger.LogInformation(200, "BankTransactions request complete");
            return Ok(result);
        }
    }
}