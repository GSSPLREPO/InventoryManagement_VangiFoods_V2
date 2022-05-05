using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace InVanWebApp.DAL
{
    public partial class InVanDBContext : DbContext
    {
        public InVanDBContext() : base("name=InVanDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<UnitMaster> UnitMasters { get; set; }
    }
}