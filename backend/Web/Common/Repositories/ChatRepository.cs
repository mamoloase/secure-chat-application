using Web.Common.Domain.Contexts;
using Web.Common.Domain.Entities;

namespace Web.Common.Repositories;
public class ChatRepository : BaseRepository<ChatEntity>
{
    public ChatRepository(ApplicationDBContext context) : base(context)
    { }
}
