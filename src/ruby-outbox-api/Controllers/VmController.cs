using Microsoft.AspNetCore.Mvc;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Dto;

namespace ruby_outbox_api.Controllers;

[ApiController]
[Route("vms")]
[Produces("application/json")]
public class VmController(
    ILogger<VmController> logger,
    IVmService vmService) : ControllerBase
{
    /// <summary>
    ///     Creates a new virtual machine for the specified customer.
    /// </summary>
    /// <remarks>
    ///     This method initiates the creation of a virtual machine for the customer identified by the
    ///     <see cref="CustomerDto.CustomerId"/> property. 
    ///     The operation is asynchronous and returns the details of the created virtual machine 
    ///     upon successful completion.
    /// </remarks>
    /// <param name="dto">The customer data transfer object containing the customer ID.</param>
    /// <returns>
    ///     An <see cref="IActionResult"/> containing the created virtual machine details 
    ///     as a <see cref="VmDto"/> with a status code of <see cref="StatusCodes.Status200OK"/>.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(VmDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddVm([FromBody] CustomerDto dto)
    {
        logger.LogDebug("Creating new Virtual Machine for the customer - {cid} request started.", dto.CustomerId);

        var vmDto = await vmService.AddVmAsync(dto.CustomerId);

        return Ok(vmDto);
    }
}
