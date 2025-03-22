using LingoMQ.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Configurations;

public class AuthorizationTypeConfiguration : IEntityTypeConfiguration<AuthorizationType>
{
    public void Configure(EntityTypeBuilder<AuthorizationType> builder)
    {
        builder.Property(x => x.Type).HasColumnName("auth_type");
        builder.Property(x => x.Value).HasColumnName("auth_value");
    }
}
