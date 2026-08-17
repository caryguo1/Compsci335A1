//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using A1Template.Models;
//using Microsoft.AspNetCore.Http;

namespace A1Template.Data
{
    public class A1DbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public A1DbContext(DbContextOptions<A1DbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public A1DbContext(DbContextOptions<A1DbContext> options) : base(options) {}
        public override int SaveChanges()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string? clientIp = httpContext?.Connection?.RemoteIpAddress?.ToString();

            var addedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is Comment);

            foreach (var entry in addedEntries)
            {
                Comment? comment = entry.Entity as Comment;

                if (comment != null)
                {
                    comment.Time = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
                    comment.IP = clientIp ?? "Unknown";
                }
            }

            // 3. Save to database normally
            return base.SaveChanges();
        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Sign> Signs { get; set; }

    }
}
