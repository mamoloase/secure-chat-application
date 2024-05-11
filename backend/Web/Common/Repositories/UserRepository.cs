using Web.Common.Domain.Contexts;
using Web.Common.Domain.Entities;

namespace Web.Common.Repositories;
public class UserRepository : BaseRepository<UserEntity>
{
    public UserRepository(ApplicationDBContext context) : base(context)
    {
    }
}
