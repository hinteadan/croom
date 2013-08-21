using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Croom.Data
{
    public interface IStoreDataAsKeyValue
    {
        Guid Save(object data);
        void SaveOrUpdate(KeyValuePair<Guid, object> entry);
        object Load(Guid id);
        IEnumerable<KeyValuePair<Guid, object>> Load();
        void Remove(Guid id);
    }
}
