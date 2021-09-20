using Microsoft.EntityFrameworkCore;
using Wirenoid.WebApi.DbItems.Models;

namespace Wirenoid.WebApi.DbItems
{
    public class CacheDbContext : DbContext
    {
        public CacheDbContext(DbContextOptions<CacheDbContext> options)
            : base(options) { }

        public DbSet<LaunchedContainer> launchedContainers { get; set; }
    }
}
