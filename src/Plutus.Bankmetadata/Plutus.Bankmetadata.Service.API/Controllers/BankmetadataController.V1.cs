using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;

namespace Plutus.Bankmetadata.Service.API.Controllers.V1
{
    /// <summary>
    /// Central controller for bank metadata
    /// </summary>
    [ApiVersion("1.0")]
    //[Produces("application/json")]
    //[Route("api/v{version:apiVersion}/[controller]/[action]/{format?}")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [FormatFilter]
    public class BankmetadataController : Controller
    {
        IBankMetadata _repo;
        private readonly ILogger<BankmetadataController> _logger;

        /// <summary>
        /// Primary constructor with DI for Logging and Repository
        /// </summary>
        public BankmetadataController(IBankMetadata repo, ILogger<BankmetadataController> logger)
        {
            _repo = repo;
            _logger = logger;
            _logger.LogInformation(200, "BankmetadataController invoked");
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
        [Route("{format?}")]
        public IActionResult BankMetadata()
        {
            List<BankMetadata> txns = new List<BankMetadata>();
            txns = _repo.GetBankMetadata().ToList();
            var result = txns;
            _logger.LogInformation(200, "BankMetadata request complete");
            return Ok(result);
        }
    }
}