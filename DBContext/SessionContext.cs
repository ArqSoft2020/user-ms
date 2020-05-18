using Microsoft.EntityFrameworkCore;
using userService.Model;

namespace userService.DBContext
{
    public class SessionContext : DbContext
    {
        public SessionContext(DbContextOptions<SessionContext> options) : base(options)
        {

        }

        public DbSet<Session> Session {get; set;}       

    }
}