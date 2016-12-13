// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Ivan">
//   Starikov Ivan, 2016
// </copyright>
// <summary>
//   MainWindow view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TestWpfAppStarikov.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;

    using TestWpfAppStarikov.Models;
    using TestWpfAppStarikov.Services.Interfaces;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules", Justification = "Reviewed. Suppression is OK here.")]
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The repository service.
        /// </summary>
        private readonly IRepositoryService m_repositoryService;

        /// <summary>
        /// The <c>ui</c> visualizer service.
        /// </summary>
        private readonly IUIVisualizerService m_uiVisualizerService;

        /// <summary>
        /// The message service.
        /// </summary>
        private readonly IMessageService m_messageService;

        /// <summary>
        /// The please wait service.
        /// </summary>
        private readonly IPleaseWaitService m_pleaseWaitService;

        /// <summary>
        /// The m_title.
        /// </summary>
        private readonly string m_title = (string)Application.Current.FindResource("MainTitle");

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="repositoryService">
        /// The repository service.
        /// </param>
        /// <param name="uiVisualizerService">
        /// The <c>ui</c> visualizer service.
        /// </param>
        /// <param name="messageService">
        /// The message service.
        /// </param>
        /// <param name="pleaseWaitService">
        /// The please wait service.
        /// </param>
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
        public override string Title => this.m_title;

        /// <summary>
        /// Gets or sets collection of clients.
        /// </summary>
        public ObservableCollection<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets collection of filtered clients.
        /// </summary>
        public ObservableCollection<Client> FilteredClients { get; set; }

        /// <summary>
        /// Gets or sets search filter.
        /// </summary>
        public string SearchFilter { get; set; }

        /// <summary>
        /// Gets or sets selected client. Used for selected item in view.
        /// </summary>
        public Client SelectedClient { get; set; }
        #endregion

        #region Commands

        /// <summary>
        /// Gets the Refresh command.
        /// </summary>
        public Command Refresh { get; private set; }

        /// <summary>
        /// Method to invoke when the Refresh command is executed.
        /// </summary>
        private async void OnRefreshExecute()
        {
            this.m_pleaseWaitService.Show();
            try
            {
                ((ICollection<Client>)this.Clients).ReplaceRange(this.m_repositoryService.GetAllClients());
                this.UpdateSearchFilter();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                this.m_pleaseWaitService.Hide();
                return;
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                this.m_pleaseWaitService.Hide();
                return;
            }
            catch (DbEntityValidationException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                this.m_pleaseWaitService.Hide();
                return;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Log.Error(ex, "sql error");
                var errorMsg = (string)Application.Current.FindResource("SqlErrorMsg");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                this.m_pleaseWaitService.Hide();
            }
        }

        /// <summary>
        /// Gets the AddClient command.
        /// </summary>
        public TaskCommand AddClient { get; private set; }

        /// <summary>
        /// Method to invoke when the AddClient command is executed.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task OnAddClientExecuteAsync()
        {
            var client = new Client { BirthDate = DateTime.Now };
            var addTitle = (string)Application.Current.FindResource("Add") ?? "Add";
            // Note that I use the type factory here because it will automatically take care of any dependencies
            // that the ClientWindowViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var familyWindowViewModel =
                typeFactory.CreateInstanceWithParametersAndAutoCompletion<ClientWindowViewModel>(client, addTitle);
            if (await this.m_uiVisualizerService.ShowDialogAsync(familyWindowViewModel) ?? false)
            {
                this.m_pleaseWaitService.Show();
                try
                {
                    this.m_repositoryService.InsertClient(client);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    this.m_pleaseWaitService.Hide();
                    return;
                }
                catch (DbUpdateException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    this.m_pleaseWaitService.Hide();
                    return;
                }
                catch (DbEntityValidationException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorAddItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    this.m_pleaseWaitService.Hide();
                    return;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("SqlErrorMsg");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    this.m_pleaseWaitService.Hide();
                    return;
                }

                Log.Info($"Client has been added: {client}");
                this.Clients.Add(client);
                this.UpdateSearchFilter();
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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task OnEditClientExecute()
        {
            // Note that I use the type factory here because it will automatically take care of any dependencies
            // that the PersonViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var editTitle = (string)Application.Current.FindResource("EditTitle") ?? "Сlient Id: {0}";
            var clientWindowViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ClientWindowViewModel>(this.SelectedClient, string.Format(editTitle, this.SelectedClient.Id));
            await this.m_uiVisualizerService.ShowDialogAsync(clientWindowViewModel);
            this.m_pleaseWaitService.Show();
            try
            {
                this.m_repositoryService.UpdateClient(this.SelectedClient);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorUpdateItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                this.m_pleaseWaitService.Hide();
                return;
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorUpdateItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                this.m_pleaseWaitService.Hide();
                return;
            }
            catch (DbEntityValidationException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("ErrorUpdateItem");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                this.m_pleaseWaitService.Hide();
                return;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Log.Error(ex);
                var errorMsg = (string)Application.Current.FindResource("SqlErrorMsg");
                var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                this.m_pleaseWaitService.Hide();
                return;
            }

            Log.Info($"Client has been edited: {this.SelectedClient}");
            this.UpdateSearchFilter();
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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task OnRemoveClientExecute()
        {
            var deleteMsg = (string)Application.Current.FindResource("DeleteItemMessage")??"Are you sure want to delete client '{0}'?";
            var deleteCaption = (string)Application.Current.FindResource("DeleteItemCaption")??"Are you sure";
            if (await this.m_messageService.ShowAsync(string.Format(deleteMsg, this.SelectedClient), 
                deleteCaption, MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
            {
                this.m_pleaseWaitService.Show();
                try
                {
                    this.m_repositoryService.DeleteClient(this.SelectedClient);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorDeleteItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    this.m_pleaseWaitService.Hide();
                    return;
                }
                catch (DbUpdateException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorDeleteItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    this.m_pleaseWaitService.Hide();
                    return;
                }
                catch (DbEntityValidationException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("ErrorDeleteItem");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    this.m_pleaseWaitService.Hide();
                    return;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Log.Error(ex);
                    var errorMsg = (string)Application.Current.FindResource("SqlErrorMsg");
                    var errorCaption = (string)Application.Current.FindResource("ErrorCaption");
                    await this.m_messageService.ShowAsync(errorMsg, icon: MessageImage.Error, caption: errorCaption);
                    this.m_pleaseWaitService.Hide();
                    return;
                }

                Log.Info($"Client has been deleted: {this.SelectedClient}");
                this.Clients.Remove(this.SelectedClient);
                this.SelectedClient = null;
                this.UpdateSearchFilter();
            }
        }

        /// <summary>
        /// Updates the filtered items.
        /// </summary>
        private void UpdateSearchFilter()
        {
            if (this.FilteredClients == null)
            {
                this.FilteredClients = new ObservableCollection<Client>();
            }

            if (string.IsNullOrWhiteSpace(this.SearchFilter))
            {
                this.m_pleaseWaitService.Show(
                    () =>
                    {
                        ((ICollection<Client>)this.FilteredClients).ReplaceRange(this.Clients);
                    });
            }
            else
            {
                var lowerSearchFilter = this.SearchFilter.ToLower();
                this.m_pleaseWaitService.Show(
                    () =>
                    {
                        ((ICollection<Client>)this.FilteredClients).ReplaceRange(
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

        /// <summary>
        /// The initialize async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override async Task InitializeAsync()
        {
            this.m_pleaseWaitService.Show();
            this.Clients = new ObservableCollection<Client>();
            this.OnRefreshExecute();
        }

        #endregion
    }
}
