using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;
using System.Linq;
using System.Data.Entity;

namespace Marina.Store.Web.Commands
{
    public class GetProductCommand : Command
    {
        private readonly StoreDbContext _db;

        public GetProductCommand(StoreDbContext db)
        {
            _db = db;
        }

        public CommandResult<Product> Execute(int id)
        {
            var product = _db.Products
                .Include(p=>p.Params)
                .Include(p=>p.Category)
                .FirstOrDefault(p => p.Id == id);
            return Result(product);
        }
    }
}