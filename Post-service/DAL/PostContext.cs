using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL
{
    public class PostContext: DbContext
    {
        public PostContext(DbContextOptions options) : base(options) { }

        public DbSet<Post> Posts { get; set; }

    }
}