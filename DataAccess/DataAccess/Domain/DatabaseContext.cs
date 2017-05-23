using System.Data.Entity;
using DataAccess.Domain.Entity;

namespace DataAccess.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("konturDB") { }

        public DbSet<TreeNode> TreeNodes { get; set; }
    }
}
