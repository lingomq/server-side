using LingoMQ.Core.Domain.Words;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LingoMQ.Infrastructure.Persistense.EntityFramework.Configurations;

public class WordInfoConfiguration : IEntityTypeConfiguration<WordInfo>
{
    public void Configure(EntityTypeBuilder<WordInfo> builder)
    {
        builder.ToTable("word_infos");
        builder.HasMany(x => x.Translations).WithMany();
        builder.OwnsOne(
            x => x.Thematics,
            t =>
            {
                t.WithOwner();
                t.Property(x => x.Category);
            }
        );
        builder.OwnsOne(
            x => x.Language,
            l =>
            {
                l.WithOwner();
                l.Property(x => x.Value);
                l.Property(x => x.Code);
                l.Property(x => x.SubCode);
            }
        );
    }
}
