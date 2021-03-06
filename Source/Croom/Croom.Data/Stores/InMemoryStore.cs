﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Croom.Data.Stores
{
    public class InMemoryStore : IStoreDataAsKeyValueLikeNothing
    {
        private readonly Dictionary<Guid, object> dataDictionary = new Dictionary<Guid, object>();

        public Guid Save(object data)
        {
            Guid id = Guid.NewGuid();
            dataDictionary.Add(id, data);
            return id;
        }

        public void SaveOrUpdate(KeyValuePair<Guid, object> entry)
        {
            if (dataDictionary.ContainsKey(entry.Key))
            {
                dataDictionary[entry.Key] = entry.Value;
                return;
            }

            dataDictionary.Add(entry.Key, entry.Value);
        }

        public object Load(Guid id)
        {
            if (!dataDictionary.ContainsKey(id))
            {
                return null;
            }
            return dataDictionary[id];
        }

        public IEnumerable<KeyValuePair<Guid, object>> Load()
        {
            return dataDictionary.Keys
                .Select(k => new KeyValuePair<Guid, object>(k, dataDictionary[k]))
                .ToArray();
        }


        public void Remove(Guid id)
        {
            dataDictionary.Remove(id);
        }
    }
}
