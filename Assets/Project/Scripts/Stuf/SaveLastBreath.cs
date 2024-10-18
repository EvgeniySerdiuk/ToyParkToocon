using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class SaveLastBreath : MonoBehaviour
    {
        private SavingWrapper saveWrapper;
        private LastBreathWrapper lastBreath;

        [Inject]
        private void Init(SavingWrapper saveWrapper, LastBreathWrapper lastBreath)
        {
            this.saveWrapper = saveWrapper;
            this.lastBreath = lastBreath;
        }

        private void OnDestroy()
        {
            lastBreath.UseAllLastBreath();
            saveWrapper.SaveWallet();
        }
    }
}