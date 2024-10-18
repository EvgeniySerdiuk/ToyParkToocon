using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;


    public class WalletSaver : MonoBehaviour, ISaveable
    {
        private Wallet wallet;
        private PlayerExperience exp;

        [Inject]
        private void Init(Wallet wallet, PlayerExperience exp)
        {
            this.wallet = wallet;
            this.exp = exp;
        }

        public JToken CaptureAsJToken()
        {
            JToken[] saved = new[] {    wallet.CaptureAsJToken(), exp.CaptureAsJToken() };
            return JToken.FromObject(saved);
        }

        public void RestoreFromJToken(JToken state)
        {
            JToken[] saved = state.ToObject<JToken[]>();
            wallet.RestoreFromJToken(saved[0]);
            exp.RestoreFromJToken(saved[1]);
        }
    }
