using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class ParamMapping : EntityTypeConfiguration<Param>
    {
        public ParamMapping()
        {
            HasKey(p => new { p.ProductId, p.Name });
            Property(p => p.ProductId).IsRequired();
            Property(p => p.Name).IsRequired().HasMaxLength(50);
            Property(p => p.Value).IsRequired().HasMaxLength(50);
        }
    }
}