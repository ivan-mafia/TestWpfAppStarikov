using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpfAppStarikov.Services
{
    using TestWpfAppStarikov.Models;

    using TestWpfAppStarikov.DbContext;

    public class RepositoryService : IRepositoryService
    {
        public RepositoryService()
        { }

        public ICollection<Client> GetAllClients()
        {
            using (ClientContext context = new ClientContext())
            {
                return Repository.Select<Client>(context).ToList();
            }
        }

        public ICollection<Client> GetClientsLastNameFiltered(string filterText)
        {
            using (ClientContext context = new ClientContext())
            {
                return Repository.Select<Client>(context).Where(c => c.LastName.Contains(filterText)).ToList();
            }
        }

        public void InsertClient(Client client)
        {
            if (client != null)
            {
                using (ClientContext context = new ClientContext())
                {
                    Repository.Insert(client, context);
                }
            }

        }

        public void InsertClient(IEnumerable<Client> clients)
        {
            if (clients != null)
            {
                using (ClientContext context = new ClientContext())
                {
                    Repository.Insert(clients, context);
                }
            }
        }

        public void UpdateClient(Client client)
        {
            using (ClientContext context = new ClientContext())
            {
                var clientDb = Repository.Select<Client>(context).FirstOrDefault(c => c.Id == client.Id);

                if (clientDb != null
                    && (clientDb.LastName != client.LastName || clientDb.FirstName != client.FirstName
                        || clientDb.BirthDate != client.BirthDate))
                {
                    clientDb.LastName = client.LastName;
                    clientDb.FirstName = client.FirstName;
                    clientDb.BirthDate = client.BirthDate;

                    Repository.Update(clientDb, context);
                }
            }
        }

        public void DeleteClient(Client client)
        {
            using (ClientContext context = new ClientContext())
            {
                var clientDb = Repository.Select<Client>(context).FirstOrDefault(c => c.Id == client.Id);

                if (clientDb != null)
                {
                    Repository.Delete(clientDb, context);
                }
            }
        }

        public bool IsClientsEmpty()
        {
            using (ClientContext context = new ClientContext())
            {
                return !Repository.Select<Client>(context).Any();
            }
        }
    }
}
