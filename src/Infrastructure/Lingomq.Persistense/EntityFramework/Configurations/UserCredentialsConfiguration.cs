using LingoMQ.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Configurations;

public class UserCredentialsConfiguration : IEntityTypeConfiguration<UserCredentials>
{
    public void Configure(EntityTypeBuilder<UserCredentials> builder)
    {
        builder.HasMany(x => x.AuthorizationTypes).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}
