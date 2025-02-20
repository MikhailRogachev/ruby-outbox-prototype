using Microsoft.AspNetCore.Mvc;

namespace ruby_outbox_api.Controllers;

[ApiController]
[Route("customers")]
[Produces("application/json")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;

}
