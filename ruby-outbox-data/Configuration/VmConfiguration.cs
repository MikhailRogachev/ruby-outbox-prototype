using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ruby_outbox_core.Contracts.Enums;
using ruby_outbox_core.Models;

namespace ruby_outbox_data.Configuration;

public class VmConfiguration : IEntityTypeConfiguration<Vm>
{
    public void Configure(EntityTypeBuilder<Vm> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.CustomerId).ValueGeneratedNever();
        builder.Property(p => p.Status).HasConversion(new EnumToStringConverter<VmStatus>());
    }
}
