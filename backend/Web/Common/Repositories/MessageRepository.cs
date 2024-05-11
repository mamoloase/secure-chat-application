using Web.Common.Domain.Contexts;
using Web.Common.Domain.Entities;

namespace Web.Common.Repositories;
public class MessageRepository : BaseRepository<MessageEntity>
{
    public MessageRepository(ApplicationDBContext context) : base(context)
    { }
}
