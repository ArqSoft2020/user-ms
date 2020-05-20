using Microsoft.EntityFrameworkCore;
using userService.Model;

namespace userService.DBContext
{
    public class SessionwContext : DbContext
    {
        public SessionwContext(DbContextOptions<SessionwContext> options) : base(options)
        {

        }

        public DbSet<Sessionw> Sessionw {get; set;}       

    }
}