using System.Threading.Tasks;

namespace TestWpfAppStarikov.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    using TestWpfAppStarikov.Models;

    public class ClientWindowViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;

        private string m_titleString = String.Empty;

        public ClientWindowViewModel(Client client, string title, IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            Argument.IsNotNull(() => client);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);

            this.Client = client;
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            m_titleString = title;
        }

        public override string Title { get { return m_titleString; } }

        /// <summary>
        /// Gets the Client.
        /// </summary>
        [Model]
        public Client Client
        {
            get { return GetValue<Client>(ClientProperty); }
            private set { SetValue(ClientProperty, value); }
        }

        /// <summary>
        /// Register the Client property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ClientProperty = RegisterProperty("Client", typeof(Client), null);

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [ViewModelToModel("Client")]
        public string FirstName
        {
            get { return GetValue<string>(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        /// <summary>
        /// Register the FirstName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FirstNameProperty = RegisterProperty("FirstName", typeof(string), null);

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [ViewModelToModel("Client")]
        public string LastName
        {
            get { return GetValue<string>(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        /// <summary>
        /// Register the LastName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LastNameProperty = RegisterProperty("LastName", typeof(string), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Client")]
        public int Id
        {
            get { return GetValue<int>(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        /// <summary>
        /// Register the Id property so it is known in the class.
        /// </summary>
        public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int), null);

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        [ViewModelToModel("Client")]
        public DateTime BirthDate
        {
            get { return GetValue<DateTime>(BirthDateProperty); }
            set { SetValue(BirthDateProperty, value); }
        }

        /// <summary>
        /// Register the BirthDate property so it is known in the class.
        /// </summary>
        public static readonly PropertyData BirthDateProperty = RegisterProperty("BirthDate", typeof(DateTime), null);
    }
}
