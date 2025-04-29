using AutoMapper;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;

namespace ruby_outbox_infrastructure.Profiles;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        // Customer mapping
        CreateMap<CustomerDto, Customer>()
            .ForMember(d => d.Id, m => m.MapFrom(r => r.CustomerId))
            .ForMember(d => d.UpdatedAt, m => m.MapFrom(r => DateTime.UtcNow));
        CreateMap<Customer, CustomerDto>()
            .ForMember(d => d.CustomerId, m => m.MapFrom(r => r.Id));

        // Vm mapping
        CreateMap<Vm, VmDto>();
    }
}
