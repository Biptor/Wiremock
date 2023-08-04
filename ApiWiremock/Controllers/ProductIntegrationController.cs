using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiWireMock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductIntegrationController : ControllerBase
    {
        private readonly ILogger<ProductIntegrationController> _logger;
        private readonly IGetDateTimeProductApiUseCase _getDateTimeProductApiUseCase;

        public ProductIntegrationController(ILogger<ProductIntegrationController> logger, IGetDateTimeProductApiUseCase useCase)
        {
            _logger = logger;
            _getDateTimeProductApiUseCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _getDateTimeProductApiUseCase.ExecuteAsync());
        }
    }
}