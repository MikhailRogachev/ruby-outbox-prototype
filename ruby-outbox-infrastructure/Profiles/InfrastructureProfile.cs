using AutoMapper;
using ruby_outbox_core.Dto;
using ruby_outbox_core.Models;

namespace ruby_outbox_infrastructure.Profiles;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<CustomerDto, Customer>()
            .ForMember(d => d.Id, m => m.MapFrom(r => r.CustomerId));
        CreateMap<Customer, CustomerDto>()
            .ForMember(d => d.CustomerId, m => m.MapFrom(r => r.Id));
    }
}
