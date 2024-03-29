﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientContext.cs" company="Ivan">
//   Starikov Ivan, 2016
// </copyright>
// <summary>
//   Defines the ClientContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestWpfAppStarikov.DbContext
{
    using System.Data.Entity;

    using TestWpfAppStarikov.Models;

    /// <summary>
    /// The client context.
    /// </summary>
    public class ClientContext : DbContext
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContext"/> class.
        /// </summary>
        public ClientContext()
            : base("clientsdb")
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        public DbSet<Client> Clients { get; set; }
        #endregion

        #region Private, Protected Methods
        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().Ignore(t => t.IsDirty).Ignore(t => t.IsReadOnly);
            base.OnModelCreating(modelBuilder);
        } 
        #endregion
    }
}
