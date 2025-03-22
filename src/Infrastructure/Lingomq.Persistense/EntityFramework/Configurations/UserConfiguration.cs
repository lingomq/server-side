using LingoMQ.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(x => x.Role).Property(x => x.Name).HasColumnName("role_name");
        builder.OwnsOne(x => x.Role).Property(x => x.Weight).HasColumnName("role_weight");
        builder.HasOne(x => x.Image).WithMany();
    }
}
