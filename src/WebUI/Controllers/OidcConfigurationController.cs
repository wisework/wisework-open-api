
using Microsoft.AspNetCore.Mvc;

namespace WW.OpenAPI.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class OidcConfigurationController : Controller
{
    private readonly ILogger<OidcConfigurationController> logger;

    public OidcConfigurationController(ILogger<OidcConfigurationController> _logger)
    {
        logger = _logger;
    }

    
    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        // ตัวอย่างการสร้างค่าตอบกลับสำหรับคำขอ OIDC configuration
        var parameters = new
        {
            ClientId = clientId,
        };

        return Ok(parameters);
    }
}
