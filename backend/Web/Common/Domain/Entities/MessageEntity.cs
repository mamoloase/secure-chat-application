using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Common.Domain.Entities;
public class MessageEntity : BaseEntity
{
    public string Message { get; set; }

    public string MediaId { get; set; }
    public MediaEntity Media { get; set; }

    public string FromId { get; set; }
    public UserEntity From { get; set; }

    public string ChatId { get; set; }
    public ChatEntity Chat { get; set; }
}

public class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.HasOne(x => x.From).WithMany();
    }
}
