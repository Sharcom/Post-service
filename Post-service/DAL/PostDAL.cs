using AL;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class PostDAL: IPostCollection
    {
        private readonly PostContext _context;

        public PostDAL(DbContext context)
        {
            _context = context as PostContext ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
