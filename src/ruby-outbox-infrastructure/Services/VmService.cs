using AutoMapper;
using Microsoft.Extensions.Logging;
using ruby_outbox_core.Contracts.Interfaces.Repositories;
using ruby_outbox_core.Contracts.Interfaces.Services;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;

namespace ruby_outbox_infrastructure.Services;

public class VmService(
    ILogger<VmService> logger,
    IVmRepository vmRepository,
    ICustomerRepository customerRepository,
    IMapper mapper) : IVmService
{
    public async Task<VmDto> AddVmAsync(Guid customerId)
    {
        // get customer
        var customer = await customerRepository.TryGetAsync(customerId);

        if (customer == null)
        {
            logger.LogInformation("The Customer {cid} doesn't exist.", customerId);
            throw new DataMisalignedException();
        }

        // create vm
        var vm = new Vm(customerId);

        logger.LogInformation("Adding Vm {vid} process started.", vm.Id);

        vm.StartVmCreation();

        vmRepository.AddVm(vm);
        await vmRepository.UnitOfWork.SaveAsync();

        return mapper.Map<VmDto>(vm);
    }

    public Task<IList<VmDto>> GetVmsByCustomerIdAsync(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public Task<VmDto?> TryGetVmByIdAsync(Guid vmId)
    {
        throw new NotImplementedException();
    }
}
