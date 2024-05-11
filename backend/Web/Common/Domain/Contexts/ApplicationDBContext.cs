using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Web.Common.Domain.Entities;

namespace Web.Common.Domain.Contexts;
public class ApplicationDBContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ChatEntity> Chats { get; set; }
    public DbSet<MediaEntity> Medias { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<SessionEntity> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        base.OnConfiguring(optionsBuilder);
    }
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }
}
