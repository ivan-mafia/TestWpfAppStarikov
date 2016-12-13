// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepositoryService.cs" company="Ivan">
//   Starikov Ivan, 2016.
// </copyright>
// <summary>
//   Defines the IRepositoryService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestWpfAppStarikov.Services.Interfaces
{
    using System.Collections.Generic;

    using TestWpfAppStarikov.Models;

    /// <summary>
    /// The RepositoryService interface.
    /// </summary>
    public interface IRepositoryService
    {
        /// <summary>
        /// The get all clients.
        /// </summary>
        /// <returns>
        /// The list of clients.
        /// </returns>
        ICollection<Client> GetAllClients();

        /// <summary>
        /// The get clients last name filtered.
        /// </summary>
        /// <param name="filterText">
        /// The filter text.
        /// </param>
        /// <returns>
        /// The list of clients filtered by last name.
        /// </returns>
        ICollection<Client> GetClientsLastNameFiltered(string filterText);

        /// <summary>
        /// The insert client.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        void InsertClient(Client client);

        /// <summary>
        /// The insert client.
        /// </summary>
        /// <param name="clients">
        /// The clients.
        /// </param>
        void InsertClient(IEnumerable<Client> clients);

        /// <summary>
        /// The update client.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        void UpdateClient(Client client);

        /// <summary>
        /// The is clients empty.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsClientsEmpty();

        /// <summary>
        /// The delete client.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        void DeleteClient(Client client);
    }
}
