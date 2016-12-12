using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.GettingStarted.DbContext
{
    using System.Data.Entity;

    using TestWpfAppStarikov.Models;

    public class ClientContext : DbContext
    {
        public ClientContext()
            : base("clientsdb")
        { }

        public DbSet<Client> Clients { get; set; }
    }
}
