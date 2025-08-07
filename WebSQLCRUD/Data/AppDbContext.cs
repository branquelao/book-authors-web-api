using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace WebSQLCRUD.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Models.AuthorModel> Authors { get; set; }
        public DbSet<Models.BookModel> Books { get; set; }
    }
}
