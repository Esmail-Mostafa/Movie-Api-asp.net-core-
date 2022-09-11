using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie.Database.Entity;

namespace Movie.Database.ApplecationContext
{
    public class ApplecationDbContext : IdentityDbContext
    {
        public ApplecationDbContext(DbContextOptions<ApplecationDbContext> opt) : base(opt)
        {

        }
        public DbSet<Genre> Genre { get; set; }

        public DbSet<Movies> Movies { get; set; }
    }
}
