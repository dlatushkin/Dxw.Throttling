//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Throttling
//{
//    public class ThrottlingStorage : IThrottlerStorage
//    {
//        private Dictionary<ThrottlingSlotKey, ThrottlingSlot> _store = new Dictionary<ThrottlingSlotKey, ThrottlingSlot>();

//        public ThrottlingSlotState Hit(ThrottlingQuota quota, ThrottlingSlotKey key)
//        {
//            var utcNow = DateTime.UtcNow;
//            ThrottlingSlot slot;

//            if (!_store.TryGetValue(key, out slot))
//            {
//                slot = new ThrottlingSlot(quota);
//            }
//            else
//            {
//                if (slot.Start.AddSeconds(key.RangeSeconds) <= utcNow) // slot is expired
//                {
//                    slot = new ThrottlingSlot { Start = utcNow, Hits = 0 };
//                }
//            }

//            slot.Hits++;
//            _store[key] = slot;

//            return slot.State;
//        }

//        public void Dispose()
//        {
//            _store.Clear();
//            _store = null;
//        }
//    }
//}
