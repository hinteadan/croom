using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Croom.Model;
using Recognos.Core;

namespace Croom.Data.Providers
{
    public class ReservationProvider
    {
        private readonly IStoreDataAsKeyValue dataStore;

        public ReservationProvider(IStoreDataAsKeyValue store)
        {
            Check.NotNull(store, "store");
            this.dataStore = store;
        }

        public IEnumerable<KeyValuePair<Guid, Reservation>> FetchAll()
        {
            return dataStore.Load().Select(d =>
                new KeyValuePair<Guid, Reservation>(d.Key, d.Value as Reservation)
                ).ToArray();
        }

        public Reservation Load(Guid id)
        {
            return dataStore.Load(id) as Reservation;
        }

        public Guid Save(Reservation newReservation)
        {
            return dataStore.Save(newReservation);
        }

        public void Save(Guid id, Reservation updatedReservation)
        {
            dataStore.SaveOrUpdate(new KeyValuePair<Guid, object>(id, updatedReservation));
        }

        public void Remove(Guid id)
        {
            dataStore.Remove(id);
        }
    }
}
