using System.Linq.Expressions;

using Web.Common.Domain.Contexts;
using Web.Common.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Web.Common.Repositories;
public class BaseRepository<EntityType> where EntityType : BaseEntity
{
    private ApplicationDBContext _context;

    public BaseRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public virtual async Task InsertAsync(EntityType entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<EntityType>().AddAsync(entity, cancellationToken);
    }
    public virtual async Task InsertAsync(IEnumerable<EntityType> entities, CancellationToken cancellationToken = default)
    {
        await _context.Set<EntityType>().AddRangeAsync(entities, cancellationToken);
    }

    public IQueryable<EntityType> FilterAsync(Expression<Func<EntityType, bool>> predicate = null,
        Func<IQueryable<EntityType>, IIncludableQueryable<EntityType, object>> include = null,
        Func<IQueryable<EntityType>, IOrderedQueryable<EntityType>> orderBy = null,
        int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<EntityType> query = GetQueryable(predicate, include);

        if (orderBy != null) query = orderBy(query);

        if (skip != null && skip.HasValue) query = query.Skip(skip.Value);

        if (take != null && take.HasValue) query = query.Take(take.Value);

        return query;
    }
    public virtual async Task UpdateAsync(EntityType entity, CancellationToken cancellationToken = default)
    {
        _context.Entry(entity).State = EntityState.Modified;

        await Task.CompletedTask;
    }
    public virtual async Task UpdateAsync(IEnumerable<EntityType> entities, CancellationToken cancellationToken = default)
    {
        _context.AttachRange(entities);
        _context.Entry(entities).State = EntityState.Modified;

        await Task.CompletedTask;
    }
    public virtual Task<EntityType> GetAsync(
        Expression<Func<EntityType, bool>> predicate = null,
        Func<IQueryable<EntityType>, IIncludableQueryable<EntityType, object>> include = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<EntityType> query = GetQueryable(predicate, include);

        return query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteAsync(EntityType entity, CancellationToken cancellationToken = default)
    {
        _context.Remove(entity);

        await Task.CompletedTask;
    }
    public virtual async Task DeleteAsync(IEnumerable<EntityType> entities, CancellationToken cancellationToken = default)
    {
        _context.RemoveRange(entities);

        await Task.CompletedTask;
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<EntityType> query = GetQueryable();

        return await query.CountAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<EntityType, bool>> predicate, CancellationToken cancellationToken = default)
    {
        IQueryable<EntityType> query = GetQueryable(predicate);

        return await query.CountAsync(predicate, cancellationToken: cancellationToken);
    }
    public virtual IQueryable<EntityType> GetQueryable(Expression<Func<EntityType, bool>> predicate = null, Func<IQueryable<EntityType>, IIncludableQueryable<EntityType, object>> include = null)
    {
        IQueryable<EntityType> query = _context.Set<EntityType>();

        if (include != null) query = include(query);

        if (predicate != null) query = query.Where(predicate);

        return query;
    }
}
