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
    /// <summary>
    /// This methods returns a customer selected by Id.
    /// </summary>
    /// <param name="customerId">Guid</param>
    /// <returns>
    ///     Status Code 200 - CustomerDto - customer found,
    ///     otherwise Status Code 204 - NoContent - customer not found.
    /// </returns>
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

    /// <summary>
    /// Adds a new customer to the system.
    /// </summary>
    /// <remarks>
    /// This method processes the provided customer information and adds it to the system. Ensure
    /// that the <paramref name="dto"/> parameter contains valid customer data before calling this method.
    /// </remarks>
    /// <param name="dto">
    ///     The customer data transfer object containing the details of the customer to be added. 
    ///     Cannot be null.
    /// </param>
    /// <returns>
    ///     An <see cref="IActionResult"/> containing the added customer details as a <see cref="CustomerDto"/> 
    ///     with a status code of <see cref="StatusCodes.Status200OK"/>.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerDto dto)
    {
        logger.LogInformation("Adding customer - {cid} request started.", dto.CustomerId);

        var customerDto = await customerService.AddCustomerAsync(dto, CancellationToken.None);

        return Ok(customerDto);
    }
}
