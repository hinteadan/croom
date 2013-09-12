using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Croom.Data.Stores
{
    public class CachedJsonFileStore : IStoreDataAsKeyValue
    {
        private readonly MemoryCache cache;
        private readonly JsonFileStore fileStore;
        private readonly string cacheIdForRepositoryEntries = "RepositoryEntries";
        private readonly Guid cacheRepositoryId = Guid.NewGuid();

        public CachedJsonFileStore()
        {
            this.fileStore = new JsonFileStore();
            cache = new MemoryCache(cacheRepositoryId.ToString());
        }
        public CachedJsonFileStore(string storeDirectoryPath)
        {
            this.fileStore = new JsonFileStore(storeDirectoryPath);
            cache = new MemoryCache(cacheRepositoryId.ToString());
        }

        public Guid Save(object data)
        {
            Guid id = Guid.NewGuid();
            fileStore.SaveOrUpdate(id, data);
            AddToCache(id, data);
            UpdateCache(cacheIdForRepositoryEntries, fileStore.GetRepositoryEntries());
            return id;
        }

        public void SaveOrUpdate(KeyValuePair<Guid, object> entry)
        {
            fileStore.SaveOrUpdate(entry);
            UpdateCache(entry.Key.ToString(), entry.Value);
        }

        public void SaveOrUpdate(Guid id, object data)
        {
            fileStore.SaveOrUpdate(id, data);
            UpdateCache(id.ToString(), data);
        }

        public object Load(Guid id)
        {
            return ReadRepositoryData(id);
        }

        private object ReadRepositoryData(Guid id)
        {
            var key = id.ToString();
            var dataFromCache = cache[key];
            var data = dataFromCache ?? fileStore.Load(id);
            if (dataFromCache == null)
            {
                AddToCache(id, data);
            }
            return data;
        }

        public IEnumerable<KeyValuePair<Guid, object>> Load()
        {
            return LazyLoad().Select(d => new KeyValuePair<Guid, object>(d.Key, d.Value.Value));
        }

        public IEnumerable<KeyValuePair<Guid, Lazy<object>>> LazyLoad()
        {
            return ReadRepositoryEntries()
                .Select(f => new KeyValuePair<Guid, Lazy<object>>(
                    Guid.Parse(f.Name),
                    new Lazy<object>(() => Load(Guid.Parse(f.Name)))
                    ));
        }

        public void Remove(Guid id)
        {
            fileStore.Remove(id);
            cache.Remove(id.ToString());
            UpdateCache(cacheIdForRepositoryEntries, fileStore.GetRepositoryEntries());
        }

        private void AddToCache(Guid id, object data)
        {
            AddToCache(id.ToString(), data);
        }

        private void AddToCache(string id, object data)
        {
            cache.Add(id, data, new CacheItemPolicy());
        }

        private void UpdateCache(string key, object newData)
        {
            if (cache.Contains(key))
            {
                cache[key] = newData;
                return;
            }

            AddToCache(key, newData);
        }

        private IEnumerable<FileInfo> ReadRepositoryEntries()
        {
            var dataFromCache = cache[cacheIdForRepositoryEntries] as IEnumerable<FileInfo>;
            var data = dataFromCache ?? fileStore.GetRepositoryEntries();
            if (dataFromCache == null)
            {
                AddToCache(cacheIdForRepositoryEntries, data);
            }
            return data;
        }
    }
}
