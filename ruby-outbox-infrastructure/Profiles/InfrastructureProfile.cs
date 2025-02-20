using AutoMapper;
using ruby_outbox_core.Models;
using ruby_outbox_infrastructure.Dto;

namespace ruby_outbox_infrastructure.Profiles;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<CustomerDto, Customer>()
            .ForMember(d => d.Id, m => m.MapFrom(r => r.CustomerId));
    }
}
