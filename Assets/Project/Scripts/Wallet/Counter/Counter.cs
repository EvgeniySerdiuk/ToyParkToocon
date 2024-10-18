using System;
using Newtonsoft.Json.Linq;

namespace Project.Scripts.Wallet
{
    public class Counter : ICounter, ISaveable
    {
        public event Action OnCountChangeEvent;

        public int Count => count;
        protected int count;

        public Counter(int startCount = 0)
        {
            count = startCount;
        }

        public bool TryChangeCountByVal(int val)
        {
            if (count + val >= 0)
            {
                 count += val;
                 OnCountChangeEvent?.Invoke();
                 return true;
            }

            return false;
        }

        public void SetCount(int val)
        {
            int newCount = val < 0 ? 0 : val;
            count = newCount;
            OnCountChangeEvent?.Invoke();
        }
        
        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(count);
        }

        public void RestoreFromJToken(JToken state)
        { 
            TryChangeCountByVal(state.ToObject<int>());
        }
    }
}