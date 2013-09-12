using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Croom.Data.Stores
{
    public class JsonFileStore : IStoreDataAsKeyValue
    {
        private readonly DirectoryInfo storeDirectory;

        public JsonFileStore()
        {
            string path = ConfigurationManager.AppSettings["JsonFileStore.StorePath"];
            if (string.IsNullOrWhiteSpace(path))
            {
                path = string.Format("{0}JsonFileStore", AppDomain.CurrentDomain.BaseDirectory);
            }

            this.storeDirectory = new DirectoryInfo(path);
            if (!storeDirectory.Exists)
            {
                storeDirectory.Create();
            }
        }
        public JsonFileStore(string storeDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(storeDirectoryPath))
            {
                throw new ArgumentException("Paramter cannot be null or empty", "storeDirectoryPath");
            }

            this.storeDirectory = new DirectoryInfo(storeDirectoryPath);
            if (!storeDirectory.Exists)
            {
                storeDirectory.Create();
            }
        }

        public Guid Save(object data)
        {
            Guid id = Guid.NewGuid();
            File.WriteAllText(
                GenerateDataFilePath(id),
                JsonConvert.SerializeObject(data)
                );
            return id;
        }

        public void SaveOrUpdate(KeyValuePair<Guid, object> entry)
        {
            File.WriteAllText(
                GenerateDataFilePath(entry.Key),
                JsonConvert.SerializeObject(entry.Value)
                );
        }

        public void SaveOrUpdate(Guid id, object data)
        {
            SaveOrUpdate(new KeyValuePair<Guid, object>(id, data));
        }

        public object Load(Guid id)
        {
            return JsonConvert.DeserializeObject(
                File.ReadAllText(GenerateDataFilePath(id))
                );
        }

        public IEnumerable<KeyValuePair<Guid, object>> Load()
        {
            return LazyLoad().Select(d => new KeyValuePair<Guid, object>(d.Key, d.Value.Value));
        }

        public IEnumerable<KeyValuePair<Guid, Lazy<object>>> LazyLoad()
        {
            return GetRepositoryEntries()
                .Select(f => new KeyValuePair<Guid, Lazy<object>>(
                    Guid.Parse(f.Name),
                    new Lazy<object>(() => Load(Guid.Parse(f.Name)))
                    ));
        }

        public void Remove(Guid id)
        {
            File.Delete(GenerateDataFilePath(id));
        }

        internal string GetRepositoryDirectoryPath()
        {
            return storeDirectory.FullName;
        }

        internal string GenerateDataFilePath(Guid id)
        {
            return string.Format("{0}\\{1}", storeDirectory.FullName, id);
        }

        internal IEnumerable<FileInfo> GetRepositoryEntries()
        {
            return storeDirectory.GetFiles();
        }
    }
}
