using System.Data.Entity.ModelConfiguration;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.DataAccess.Mappings
{
    public class CategoryMapping : EntityTypeConfiguration<Category>
    {
        public CategoryMapping()
        {
            Property(a => a.Name).IsRequired().HasMaxLength(100);
            Property(a => a.Description).IsOptional().HasMaxLength(500);
            Property(a => a.Picture).IsOptional().HasMaxLength(100);
        }
    }
}