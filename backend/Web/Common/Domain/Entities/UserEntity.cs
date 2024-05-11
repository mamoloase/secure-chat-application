using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Common.Domain.Entities;
public class UserEntity : BaseEntity
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Description { get; set; }
    public string VerifivationCode { get; set; }
    public DateTime RequestVerificationCodeAt { get; set; }

    public List<ChatEntity> Chats { get; set; } = new();
    public List<MediaEntity> Profiles { get; set; } = new();
    public List<SessionEntity> Sessions { get; set; } = new();
}

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasIndex(x => x.Phone).IsUnique();

        builder.HasMany(x => x.Chats).WithMany(x => x.Users);
    }
}