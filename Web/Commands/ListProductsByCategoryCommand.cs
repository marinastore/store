using System.Linq;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Infrastructure.Commands;
using Marina.Store.Web.Models;
using System.Data.Entity;

namespace Marina.Store.Web.Commands
{
    public class ListProductsByCategoryCommand : Command
    {
        private readonly StoreDbContext _db;

        public ListProductsByCategoryCommand(StoreDbContext db)
        {
            _db = db;
        }

        public Result<PartialCollection<Product>> Execute(int categoryId, int skip = 0, int top = 25)
        {
            var query = _db.Products.Include(p=>p.Params)
                .Where(p => p.Category.Id == categoryId);

            var products = query
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(top)
                .ToArray();

            var total = query.Count();
            
            return new PartialCollection<Product>(total, products);
        }
    }
}