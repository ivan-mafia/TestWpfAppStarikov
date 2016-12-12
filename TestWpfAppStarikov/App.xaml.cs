namespace TestWpfAppStarikov
{
    using System.Globalization;
    using System.Windows;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Windows;

    using TestWpfAppStarikov.Services;

    using TestWpfAppStarikov.ViewModels;
    using TestWpfAppStarikov.Views;

    using WPF.GettingStarted.DbContext;
    using WPF.GettingStarted.Services;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            Catel.Logging.LogManager.AddDebugListener();
#endif
            //var languageService = ServiceLocator.Default.ResolveType<ILanguageService>();

            //// Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            //// of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            //// we use .CurrentCulture for the sake of the demo
            //languageService.PreferredCulture = CultureInfo.CurrentCulture;
            //languageService.FallbackCulture = new CultureInfo("ru-RU");

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterType<IRepositoryService, RepositoryService>();
            serviceLocator.RegisterType<IFamilyService, FamilyService>();

            var dependencyResolver = this.GetDependencyResolver();
            var repositoryService = dependencyResolver.Resolve<IRepositoryService>();
            var messageService = dependencyResolver.Resolve<IMessageService>();
            var navigationService = dependencyResolver.Resolve<IUIVisualizerService>();

            if (repositoryService.IsClientsEmpty())
            {
                repositoryService.InsertClient(TestData.Clients());
            }

            base.OnStartup(e);
        }
    }
}