using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class OrderMapping : EntityTypeConfiguration<Order>
    {
        public OrderMapping()
        {
            HasKey(o => o.Id);
            HasMany(o => o.Lines);
            HasRequired(o => o.Address);
            Property(o => o.Phone).IsRequired();
            Property(o => o.Comment).IsOptional().HasMaxLength(200);
            Property(o => o.CreateDate).IsRequired();
            Ignore(o => o.Total);
            HasOptional(o => o.User);
        }
    }
}