namespace TestWpfAppStarikov.Services
{
    using System.Collections.Generic;

    using TestWpfAppStarikov.Models;

    public interface IFamilyService
    {
        IEnumerable<Client> LoadFamilies();

        void SaveFamilies(IEnumerable<Client> families);
    }
}
