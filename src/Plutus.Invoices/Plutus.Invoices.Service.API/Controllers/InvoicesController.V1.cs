using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;

namespace Plutus.Invoices.Service.API.Controllers.V1
{
    /// <summary>
    /// Central controller for bank metadata
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [FormatFilter]
    public class InvoicesController : Controller
    {
        IInvoice _repo;
        private readonly ILogger<InvoicesController> _logger;

        /// <summary>
        /// Primary constructor with DI for Logging and Repository
        /// </summary>
        public InvoicesController(IInvoice repo, ILogger<InvoicesController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogInformation(200, "InvoicesController invoked");
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
        public IActionResult Invoices()
        {
            List<Invoice> txns = new List<Invoice>();
            txns = _repo.GetInvoices().ToList();
            var result = txns;
            _logger.LogInformation(200, "Invoices request complete");
            return Ok(result);
        }
    }
}