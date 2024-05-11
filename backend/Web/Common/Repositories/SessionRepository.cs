using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Common.Domain.Contexts;
using Web.Common.Domain.Entities;

namespace Web.Common.Repositories;
public class SessionRepository : BaseRepository<SessionEntity>
{
    public SessionRepository(ApplicationDBContext context) : base(context)
    {

    }
}
