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
    [HttpPost]
    [ProducesResponseType(typeof(VmDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddVm([FromBody] CustomerDto dto)
    {
        logger.LogDebug("Creating new Virtual Machine for the customer - {cid} request started.", dto.CustomerId);

        var vmDto = await vmService.AddVmAsync(dto.CustomerId);

        return Ok(vmDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetVms()
    {
        //var dto = await azureVmService.GetVirtualMachinesAsync();

        //await eventProducer.PublishAsync();




        return Ok();
    }
}
