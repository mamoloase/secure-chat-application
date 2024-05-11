using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Common.Domain.Entities;
public class SessionEntity : BaseEntity
{
    public string Token { get; set; }
    public string UserId { get; set; }
    public UserEntity User { get; set; }

}

public class SessionEntityConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.HasOne(x => x.User).WithMany(x => x.Sessions);
    }
}
