using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursov
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Guides> Guides { get; set; }

        public DbSet<Trip> Trip { get; set; }
        public DbSet<Login> Login { get; set; }
    }
}
