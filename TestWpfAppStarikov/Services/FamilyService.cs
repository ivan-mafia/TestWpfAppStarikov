namespace TestWpfAppStarikov.Services
{
    using System.Collections.Generic;
    using System.IO;
    using Catel.Collections;
    using Catel.Data;
    using Catel.Runtime.Serialization.Xml;

    using TestWpfAppStarikov.Models;

    public class FamilyService : IFamilyService
    {
        private readonly string _path;

        public FamilyService()
        {
            string directory = Catel.IO.Path.GetApplicationDataDirectory("CatenaLogic", "WPF.GettingStarted");
            Directory.CreateDirectory(directory);

            _path = Path.Combine(directory, "Client.xml");
        }

        public IEnumerable<Client> LoadFamilies()
        {
            if (!File.Exists(_path))
            {
                return new Client[] { };
            }

            using (var fileStream = File.Open(_path, FileMode.Open))
            {
                var settings = Settings.Load(fileStream, SerializationMode.Xml, new XmlSerializationConfiguration());
                return settings.Families;
            }
        }

        public void SaveFamilies(IEnumerable<Client> families)
        {
            var settings = new Settings();
            ((ICollection<Client>)settings.Families).ReplaceRange(families);
            settings.Save(_path, SerializationMode.Xml, new XmlSerializationConfiguration());
        }
    }
}
