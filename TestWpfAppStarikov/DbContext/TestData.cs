// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestData.cs" company="Ivan ">
//   Starikov Ivan, 2016
// </copyright>
// <summary>
//   Defines the TestData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestWpfAppStarikov.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using TestWpfAppStarikov.Models;

    /// <summary>
    /// The test data for entity context.
    /// </summary>
    public class TestData
    {
        /// <summary>
        /// Initializes list of clients.
        /// </summary>
        /// <returns>
        /// The list of clients.
        /// </returns>
        public static IEnumerable<Client> Clients()
        {
            var clients = new Collection<Client>
                              {
                                  new Client
                                      {
                                          FirstName = "Эмма",
                                          LastName = "Никифорова",
                                          BirthDate = new DateTime(1989, 9, 17)
                                      },
                                  new Client
                                      {
                                          FirstName = "Анна",
                                          LastName = "Григорьева",
                                          BirthDate = new DateTime(1964, 1, 1)
                                      }
                              };
            return clients;
        }
    }
}
