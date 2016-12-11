namespace TestWpfAppStarikov.Models
{
    using System.Collections.ObjectModel;
    using Catel.Data;

    public class Settings : SavableModelBase<Settings>
    {
        /// <summary>
        /// Gets or sets all the families.
        /// </summary>
        public ObservableCollection<Client> Families
        {
            get { return GetValue<ObservableCollection<Client>>(FamiliesProperty); }
            set { SetValue(FamiliesProperty, value); }
        }

        /// <summary>
        /// Register the Clients property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FamiliesProperty = RegisterProperty("Clients", typeof(ObservableCollection<Client>), () => new ObservableCollection<Client>());
    }
}
