// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryService.cs" company="Ivan">
//   Starikov Ivan, 2016
// </copyright>
// <summary>
//   Defines the RepositoryService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestWpfAppStarikov.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using TestWpfAppStarikov.DbContext;
    using TestWpfAppStarikov.Models;
    using TestWpfAppStarikov.Services.Interfaces;

    /// <summary>
    /// The repository service.
    /// </summary>
    public class RepositoryService : IRepositoryService
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryService"/> class.
        /// </summary>
        // ReSharper disable once EmptyConstructor
        public RepositoryService()
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The get all clients.
        /// </summary>
        /// <returns>
        /// The list of all clients.
        /// </returns>
        public ICollection<Client> GetAllClients()
        {
            using (ClientContext context = new ClientContext())
            {
                return Repository.Select<Client>(context).ToList();
            }
        }

        /// <summary>
        /// The get clients last name filtered.
        /// </summary>
        /// <param name="filterText">
        /// The filter text.
        /// </param>
        /// <returns>
        /// The list of all clients filtered by last name.
        /// </returns>
        public ICollection<Client> GetClientsLastNameFiltered(string filterText)
        {
            using (ClientContext context = new ClientContext())
            {
                return Repository.Select<Client>(context).Where(c => c.LastName.Contains(filterText)).ToList();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Inserts the client.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
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

        /// <summary>
        /// Inserts the list of client.
        /// </summary>
        /// <param name="clients">
        /// The list of clients.
        /// </param>
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

        /// <summary>
        /// Updates client entity.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
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

        /// <summary>
        /// Deletes client.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
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

        /// <summary>
        /// The is clients empty.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsClientsEmpty()
        {
            using (ClientContext context = new ClientContext())
            {
                return !Repository.Select<Client>(context).Any();
            }
        }
        #endregion
    }
}
