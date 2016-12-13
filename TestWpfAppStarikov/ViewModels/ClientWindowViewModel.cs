// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientWindowViewModel.cs" company="Ivan">
//   Starikov Ivan, 2016
// </copyright>
// <summary>
//   Defines the ClientWindowViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestWpfAppStarikov.ViewModels
{
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Catel.Services;

    using TestWpfAppStarikov.Models;

    /// <summary>
    /// The client window view model.
    /// </summary>
    public class ClientWindowViewModel : ViewModelBase
    {
        #region Private, Protected Fields
        /// <summary>
        /// The <c>ui</c> visualizer service.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly IUIVisualizerService m_uiVisualizerService;

        /// <summary>
        /// The message service.
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly IMessageService m_messageService;

        /// <summary>
        /// The title string.
        /// </summary>
        private readonly string m_titleString;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientWindowViewModel"/> class.
        /// </summary>
        /// <param name="client">
        /// Selected client.
        /// </param>
        /// <param name="title">
        /// Window title.
        /// </param>
        /// <param name="uiVisualizerService">
        /// The <c>ui</c> visualizer service.
        /// </param>
        /// <param name="messageService">
        /// The message service.
        /// </param>
        public ClientWindowViewModel(Client client, string title, IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            Argument.IsNotNull(() => client);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);

            this.Client = client;
            this.m_uiVisualizerService = uiVisualizerService;
            this.m_messageService = messageService;
            this.m_titleString = title;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title => this.m_titleString;

        /// <summary>
        /// Gets or sets client. Expose attribute used to expose FirstName, LastName, Id, BirthDate.
        /// </summary>
        [Model]
        [Expose("FirstName")]
        [Expose("LastName")]
        [Expose("Id")]
        [Expose("BirthDate")]
        public Client Client { get; set; }
        #endregion
    }
}
