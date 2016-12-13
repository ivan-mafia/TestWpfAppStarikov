using System.Threading.Tasks;

namespace TestWpfAppStarikov.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    using Catel;
    using Catel.Collections;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;

    using TestWpfAppStarikov.Models;
    using TestWpfAppStarikov.Services;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IRepositoryService m_repositoryService;
        private readonly IUIVisualizerService m_uiVisualizerService;
        private readonly IMessageService m_messageService;
        private readonly IPleaseWaitService m_pleaseWaitService;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private string m_title = (string)Application.Current.FindResource("MainTitle");

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(IRepositoryService repositoryService, IUIVisualizerService uiVisualizerService, IMessageService messageService, IPleaseWaitService pleaseWaitService)
        {
            Argument.IsNotNull(() => repositoryService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => pleaseWaitService);

            this.m_repositoryService = repositoryService;
            this.m_uiVisualizerService = uiVisualizerService;
            this.m_messageService = messageService;
            this.m_pleaseWaitService = pleaseWaitService;

            this.AddClient = new TaskCommand(this.OnAddClientExecuteAsync);
            this.Refresh = new Command(this.OnRefreshExecute);
            this.EditClient = new TaskCommand(this.OnEditClientExecute, this.OnEditClientCanExecute);
            this.RemoveClient = new TaskCommand(this.OnRemoveClientExecute, this.OnRemoveClientCanExecute);
        }

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return m_title; } }

        public ObservableCollection<Client> Clients { get; set; }

        public ObservableCollection<Client> FilteredClients { get; set; }

        public string SearchFilter { get; set; }

        public Client SelectedClient { get; set; }


        /*/// <summary>
        /// Gets the clients.
        /// </summary>
        public ObservableCollection<Client> Clients
        {
            get { return GetValue<ObservableCollection<Client>>(ClientProperty); }
            private set { SetValue(ClientProperty, value); }
        }

        /// <summary>
        /// Register the Clients property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ClientProperty = RegisterProperty("Clients", typeof(ObservableCollection<Client>), null);

        /// <summary>
        /// Gets the filtered clients.
        /// </summary>
        public ObservableCollection<Client> FilteredClients
        {
            get { return GetValue<ObservableCollection<Client>>(FilteredClientsProperty); }
            private set { SetValue(FilteredClientsProperty, value); }
        }

        /// <summary>
        /// Register the FilteredClients property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FilteredClientsProperty = RegisterProperty("FilteredClients", typeof(ObservableCollection<Client>));

        /// <summary>
        /// Gets or sets the search filter.
        /// </summary>
        public string SearchFilter
        {
            get { return GetValue<string>(SearchFilterProperty); }
            set { SetValue(SearchFilterProperty, value); }
        }

        /// <summary>
        /// Register the SearchFilter property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SearchFilterProperty = RegisterProperty("SearchFilter", typeof(string), null, 
            (sender, e) => ((MainWindowViewModel)sender).UpdateSearchFilter());

        /// <summary>
        /// Gets or sets the selected Client.
        /// </summary>
        public Client SelectedClient
        {
            get { return GetValue<Client>(SelectedClientProperty); }
            set { SetValue(SelectedClientProperty, value); }
        }

        /// <summary>
        /// Register the SelectedClient property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedClientProperty = RegisterProperty("SelectedClient", typeof(Client),null);
        */
        #endregion

        #region Commands
        /// <summary>
            /// Gets the Refresh command.
            /// </summary>
        public Command Refresh { get; private set; }

        /// <summary>
        /// Method to invoke when the Refresh command is executed.
        /// </summary>
        private void OnRefreshExecute()
        {
            m_pleaseWaitService.Show();
            ((ICollection<Client>)this.Clients).ReplaceRange(m_repositoryService.GetAllClients());
            UpdateSearchFilter();
            //((ICollection<Client>)this.Clients).ReplaceRange(m_repositoryService.GetAllClients());
        }

        /// <summary>
        /// Gets the AddClient command.
        /// </summary>
        public TaskCommand AddClient { get; private set; }

        /// <summary>
        /// Method to invoke when the AddClient command is executed.
        /// </summary>
        private async Task OnAddClientExecuteAsync()
        {
            var client = new Client();
            client.BirthDate = DateTime.Now;
            // Note that we use the type factory here because it will automatically take care of any dependencies
            // that the ClientWindowViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var familyWindowViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ClientWindowViewModel>(client, "Add Item");
            if (await this.m_uiVisualizerService.ShowDialogAsync(familyWindowViewModel) ?? false)
            {
                m_pleaseWaitService.Show();
                try
                {
                    m_repositoryService.InsertClient(client);
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    m_pleaseWaitService.Hide();
                    return;
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    m_pleaseWaitService.Hide();
                    return;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    m_pleaseWaitService.Hide();
                    return;
                }
                Log.Info("Client has been added: " + client.ToString());
                this.Clients.Add(client);
                UpdateSearchFilter();
            }
        }

        /// <summary>
        /// Gets the EditClient command.
        /// </summary>
        public TaskCommand EditClient { get; private set; }

        /// <summary>
        /// Method to check whether the EditClient command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnEditClientCanExecute()
        {
            return this.SelectedClient != null;
        }

        /// <summary>
        /// Method to invoke when the EditClient command is executed.
        /// </summary>
        private async Task OnEditClientExecute()
        {
            // Note that we use the type factory here because it will automatically take care of any dependencies
            // that the PersonViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var clientWindowViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ClientWindowViewModel>(this.SelectedClient, "Edit Item");
            await this.m_uiVisualizerService.ShowDialogAsync(clientWindowViewModel);
            m_pleaseWaitService.Show();
            try
            {
                m_repositoryService.UpdateClient(this.SelectedClient);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorUpdateItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                m_pleaseWaitService.Hide();
                return;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorUpdateItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                m_pleaseWaitService.Hide();
                return;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorUpdateItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                m_pleaseWaitService.Hide();
                return;
            }
            Log.Info("Client has been edited: " + this.SelectedClient.ToString());
            UpdateSearchFilter();
        }

        /// <summary>
        /// Gets the RemoveClient command.
        /// </summary>
        public TaskCommand RemoveClient { get; private set; }

        /// <summary>
        /// Method to check whether the RemoveClient command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRemoveClientCanExecute()
        {
            return this.SelectedClient != null;
        }

        /// <summary>
        /// Method to invoke when the RemoveClient command is executed.
        /// </summary>
        private async Task OnRemoveClientExecute()
        {
            var deleteMsg = (string)Application.Current.FindResource("DeleteItemMessage");
            var deleteCaption = (string)Application.Current.FindResource("DeleteItemCaption");
            if (await this.m_messageService.ShowAsync(string.Format(deleteMsg, this.SelectedClient),
                deleteCaption, MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
            {
                m_pleaseWaitService.Show();
                try
                {
                    m_repositoryService.DeleteClient(this.SelectedClient);
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorDeleteItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    m_pleaseWaitService.Hide();
                    return;
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorDeleteItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    m_pleaseWaitService.Hide();
                    return;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorDeleteItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    m_pleaseWaitService.Hide();
                    return;
                }
                Log.Info("Client has been deleted: " + this.SelectedClient.ToString());
                this.Clients.Remove(this.SelectedClient);
                this.SelectedClient = null;
                UpdateSearchFilter();
            }
        }

        /// <summary>
        /// Updates the filtered items.
        /// </summary>
        private void UpdateSearchFilter()
        {
            if (FilteredClients == null)
            {
                FilteredClients = new ObservableCollection<Client>();
            }

            if (string.IsNullOrWhiteSpace(SearchFilter))
            {
                m_pleaseWaitService.Show(
                    () =>
                        {
                            ((ICollection<Client>)FilteredClients).ReplaceRange(this.Clients);
                        });
            }
            else
            {
                var lowerSearchFilter = SearchFilter.ToLower();
                this.m_pleaseWaitService.Show(
                    () =>
                        {
                            ((ICollection<Client>)FilteredClients).ReplaceRange(
                                from client in this.Clients
                                where
                                    !string.IsNullOrWhiteSpace(client.LastName)
                                    && client.LastName.ToLower().Contains(lowerSearchFilter)
                                select client);
                        });
            }
        }
        #endregion

        #region Methods

        protected override async Task InitializeAsync()
        {
            //var families = _repositoryService.LoadFamilies();
            m_pleaseWaitService.Show();
            this.Clients = new ObservableCollection<Client>();
            ((ICollection<Client>)this.Clients).ReplaceRange(m_repositoryService.GetAllClients());

            UpdateSearchFilter();
        }

        //protected override async Task CloseAsync()
        //{
        //    //this.m_repositoryService.(this.Clients);
        //}

        #endregion
    }
}
