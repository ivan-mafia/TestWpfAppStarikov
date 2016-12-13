namespace TestWpfAppStarikov
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Globalization;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Catel.Windows;

    using TestWpfAppStarikov.DbContext;
    using TestWpfAppStarikov.Services;
    using TestWpfAppStarikov.Services.Interfaces;
    using TestWpfAppStarikov.ViewModels;
    using TestWpfAppStarikov.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
        Catel.Logging.LogManager.AddDebugListener();
#endif

            Log.Info("Starting application...");
            //var languageService = ServiceLocator.Default.ResolveType<ILanguageService>();

            //// Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            //// of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            //// we use .CurrentCulture for the sake of the demo
            //languageService.PreferredCulture = CultureInfo.CurrentCulture;
            //languageService.FallbackCulture = new CultureInfo("ru-RU");

            //StyleHelper.CreateStyleForwardersForDefaultStyles();

            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterType<IRepositoryService, RepositoryService>();

            var dependencyResolver = this.GetDependencyResolver();
            var repositoryService = dependencyResolver.Resolve<IRepositoryService>();
            var messageService = dependencyResolver.Resolve<IMessageService>();
            var navigationService = dependencyResolver.Resolve<IUIVisualizerService>();

            try
            {
                if (repositoryService.IsClientsEmpty())
                {
                    Log.Info("Clients table empty. Insert test data.");
                    repositoryService.InsertClient(TestData.Clients());
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex);
            }
            catch (DbEntityValidationException ex)
            {
                Log.Error(ex);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Log.Error(ex,"sql error");
            }

            Log.Info("Calling base.OnStartup");

            base.OnStartup(e);
        }
    }
}