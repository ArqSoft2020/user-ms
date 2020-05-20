using Microsoft.EntityFrameworkCore;
using userService.Model;

namespace userService.DBContext
{
    public class SessionmContext : DbContext
    {
        public SessionmContext(DbContextOptions<SessionmContext> options) : base(options)
        {

        }

        public DbSet<Sessionm> Sessionm {get; set;}       

    }
}