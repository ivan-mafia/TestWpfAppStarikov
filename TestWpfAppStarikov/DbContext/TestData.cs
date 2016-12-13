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
        #region Public Methods
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
                                      },
                                  new Client
                                      {
                                          FirstName = "Богдан",
                                          LastName = "Беляев",
                                          BirthDate = new DateTime(1984, 1, 10)
                                      },
                                  new Client
                                      {
                                          FirstName = "Федосей",
                                          LastName = "Коновалов",
                                          BirthDate = new DateTime(1968, 6, 1)
                                      },
                                  new Client
                                      {
                                          FirstName = "Леонид",
                                          LastName = "Абрамов",
                                          BirthDate = new DateTime(1994, 10, 9)
                                      },
                                  new Client
                                      {
                                          FirstName = "Регина",
                                          LastName = "Корнилова",
                                          BirthDate = new DateTime(1964, 9, 9)
                                      },
                                  new Client
                                      {
                                          FirstName = "Иван",
                                          LastName = "Власов",
                                          BirthDate = new DateTime(1964, 7, 3)
                                      }
                              };
            return clients;
        }
        #endregion
    }
}
