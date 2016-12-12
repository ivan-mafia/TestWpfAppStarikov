namespace TestWpfAppStarikov.Services
{
    using System.Collections.Generic;

    using TestWpfAppStarikov.Models;

    public interface IRepositoryService
    {
        ICollection<Client> GetAllClients();

        ICollection<Client> GetClientsLastNameFiltered(string filterText);

        void InsertClient(Client client);

        void InsertClient(IEnumerable<Client> clients);

        void UpdateClient(Client client);

        bool IsClientsEmpty();

        void DeleteClient(Client client);
    }
}