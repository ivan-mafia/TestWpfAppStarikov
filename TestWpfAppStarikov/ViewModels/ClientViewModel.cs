// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientViewModel.cs" company="Ivan">
//   Starikov Ivan,  2016
// </copyright>
// <summary>
//   Defines the ClientViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestWpfAppStarikov.ViewModels
{
    using System;

    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using Catel.MVVM;

    using TestWpfAppStarikov.Models;

    /// <summary>
    /// The client view model.
    /// </summary>
    [NoWeaving]
    public class ClientViewModel : ViewModelBase
    {
        #region Public Static Properties
        /// <summary>
        /// Register the Client property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ClientProperty = RegisterProperty("Client", typeof(Client));

        /// <summary>
        /// Register the FirstName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FirstNameProperty = RegisterProperty("FirstName", typeof(string));

        /// <summary>
        /// Register the LastName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LastNameProperty = RegisterProperty("LastName", typeof(string));

        /// <summary>
        /// Register the Id property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

        /// <summary>
        /// Register the BirthDate property so it is known in the class.
        /// </summary>
        public static readonly PropertyData BirthDateProperty = RegisterProperty("BirthDate", typeof(DateTime));
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientViewModel"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        public ClientViewModel(Client client)
        {
            Argument.IsNotNull(() => client);

            this.Client = client;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the Client.
        /// </summary>
        [Model]
        public Client Client
        {
            get { return this.GetValue<Client>(ClientProperty); }
            private set { this.SetValue(ClientProperty, value); }
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [ViewModelToModel("Client")]
        public string FirstName
        {
            get { return this.GetValue<string>(FirstNameProperty); }
            set { this.SetValue(FirstNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [ViewModelToModel("Client")]
        public string LastName
        {
            get { return this.GetValue<string>(LastNameProperty); }
            set { this.SetValue(LastNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Client")]
        public int Id
        {
            get { return this.GetValue<int>(IdProperty); }
            set { this.SetValue(IdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Client")]
        public DateTime BirthDate
        {
            get { return this.GetValue<DateTime>(BirthDateProperty); }
            set { this.SetValue(BirthDateProperty, value); }
        }
        #endregion
    }
}
