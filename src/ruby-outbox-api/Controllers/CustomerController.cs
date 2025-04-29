using Microsoft.AspNetCore.Mvc;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Dto;

namespace ruby_outbox_api.Controllers;

[ApiController]
[Route("customers")]
[Produces("application/json")]
public class CustomerController(
    ILogger<CustomerController> logger,
    ICustomerService customerService
    ) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomer(Guid customerId)
    {
        logger.LogInformation("Get customer - {cid} request started.", customerId);

        var customerDto = await customerService.GetCustomerDtoAsync(customerId, CancellationToken.None);
        if (customerDto == null)
            return NoContent();

        return Ok(customerDto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerDto dto)
    {
        logger.LogInformation("Adding customer - {cid} request started.", dto.CustomerId);

        var customerDto = await customerService.AddCustomerAsync(dto, CancellationToken.None);

        return Ok(customerDto);
    }
}
