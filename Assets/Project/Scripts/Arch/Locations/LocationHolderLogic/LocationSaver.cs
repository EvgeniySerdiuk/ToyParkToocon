using Newtonsoft.Json.Linq;
using UnityEngine;

    public class LocationSaver : MonoBehaviour, ISaveable
    {
        [SerializeField] private BuyLocation[] buyLocations;
        
        public JToken CaptureAsJToken()
        {
            JToken[] locationStates = new JToken[buyLocations.Length];
            for (int i = 0; i < buyLocations.Length; i++) { }
                //[i] = buyLocations[i].CaptureAsJToken();
            return JToken.FromObject(locationStates);
        }

        public void RestoreFromJToken(JToken state)
        {
            JToken[] states = state.ToObject<JToken[]>();
            for (int i = 0; i < buyLocations.Length && i < states.Length; i++) { }
                //buyLocations[i].RestoreFromJToken(state[i]);
        }
    }