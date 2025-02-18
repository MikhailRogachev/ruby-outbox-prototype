using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ruby_outbox_core.Models;

namespace ruby_outbox_data.Configuration;

public class CloudProcessConfiguration : IEntityTypeConfiguration<CloudProcess>
{
    public void Configure(EntityTypeBuilder<CloudProcess> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedNever();
    }
}
