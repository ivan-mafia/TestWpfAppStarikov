using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpfAppStarikov.DbContext
{
    using System.Collections.ObjectModel;

    using TestWpfAppStarikov.Models;

    public class TestData
    {
        public static IEnumerable<Client> Clients()
        {
            var clients = new Collection<Client>
                              {
                                  new Client {FirstName = "Эмма" , LastName = "Никифорова", BirthDate = new DateTime(1989,9,17)},
                                  new Client {FirstName = "Анна" , LastName = "Григорьева", BirthDate = new DateTime(1964,1,1)}
                              };
            return clients;
        } 
    }
}
