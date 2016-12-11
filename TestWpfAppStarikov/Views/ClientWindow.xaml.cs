namespace TestWpfAppStarikov.Views
{
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ClientWindow.xaml.
    /// </summary>
    public partial class ClientWindow : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientWindow"/> class.
        /// </summary>
        public ClientWindow()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public ClientWindow(ClientWindowViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
