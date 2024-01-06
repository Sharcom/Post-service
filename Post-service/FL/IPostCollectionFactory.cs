using Microsoft.EntityFrameworkCore;
using AL;
using DAL;

namespace FL
{
    public static class IPostCollectionFactory
    {
        public static IPostCollection Get(DbContext context)
        {
            return new PostDAL(context);
        }
    }
}