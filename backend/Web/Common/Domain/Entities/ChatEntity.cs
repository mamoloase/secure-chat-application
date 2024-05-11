using Web.Common.Domain.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Common.Domain.Entities;
public class ChatEntity : BaseEntity
{
    public string Name { get; set; }
    public ChatType Type { get; set; }

    public string OwnerId { get; set; }
    public UserEntity Owner { get; set; }

    public List<UserEntity> Users { get; set; }
    public List<MessageEntity> Messages { get; set; }
}
public class ChatEntityConfiguration : IEntityTypeConfiguration<ChatEntity>
{
    public void Configure(EntityTypeBuilder<ChatEntity> builder)
    {
        builder.HasOne(x => x.Owner).WithMany();
        builder.HasMany(x => x.Users).WithMany(x => x.Chats);
        builder.HasMany(x => x.Messages).WithOne(x => x.Chat);
    }
}

