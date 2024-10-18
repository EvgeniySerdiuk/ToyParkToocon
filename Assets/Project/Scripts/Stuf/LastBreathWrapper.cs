using System.Collections.Generic;
using Project.Scripts.Interfaces;

namespace Project.Scripts
{
    public class LastBreathWrapper : ILastBreathWrapper
    {
        private List<IHaveLastBreath> items;

        public LastBreathWrapper() 
        {
            items = new List<IHaveLastBreath>();
        }

        public void Add(IHaveLastBreath lastBreath)
        {
            if (items.Contains(lastBreath))
                return;
            items.Add(lastBreath);
        }

        public void Remove(IHaveLastBreath lastBreath)
        {
            if (items.Contains(lastBreath))
                items.Remove(lastBreath);
        }
        
        public void UseAllLastBreath()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null) continue;
                items[i].LastBreath();
            }
            items.Clear();
        }
    }
}