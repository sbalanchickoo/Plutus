using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;

namespace Plutus.Expenses.Service.API.Controllers.V1
{
    /// <summary>
    /// Central controller for bank metadata
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [FormatFilter]
    public class ExpensesController : Controller
    {
        IExpense _repo;
        private readonly ILogger<ExpensesController> _logger;

        /// <summary>
        /// Primary constructor with DI for Logging and Repository
        /// </summary>
        public ExpensesController(IExpense repo, ILogger<ExpensesController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogInformation(200, "ExpensesController invoked");
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
        /// Primary action, returns Bank metadata
        /// </summary>
        [HttpGet]
        [Route("[action]/{format?}")]
        [Route("[action]")]
        [Route("")]
        public IActionResult Expenses()
        {
            List<Expense> txns = new List<Expense>();
            txns = _repo.GetExpenses().ToList();
            var result = txns;
            _logger.LogInformation(200, "Expenses request complete");
            return Ok(result);
        }
    }
}