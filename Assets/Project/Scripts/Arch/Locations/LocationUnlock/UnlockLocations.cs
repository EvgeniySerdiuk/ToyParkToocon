using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Project.Scripts;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SaveableEntity))]
public class UnlockLocations : MonoBehaviour, ISaveable
{
    [SerializeField] private int[] expCountToUnlock;
    [SerializeField] private BuyLocation[] objectToSpawn;

    private PlayerExperience playerExperience;
    private bool[] usedBuyLocations;
    private bool[] sentEvents;
    private int currentIndex;

    [Inject]
    private void Init(PlayerExperience exp)
    {
        playerExperience = exp;
        int n = objectToSpawn.Length;
        usedBuyLocations = new bool[n];
        usedBuyLocations[0] = true;
        sentEvents = new bool[n];
    }

    private void Start()
    {
        UnlockAll();
    }

    private void UnlockAll()
    {
         int expCount = playerExperience.ExperienceCounter.Count;
         for (int i = 0; i < expCountToUnlock.Length; i++)
         {
             if(expCount < expCountToUnlock[i])
                 return;
             if(objectToSpawn[i] != null)
             {
                 if (usedBuyLocations[i])
                     objectToSpawn[i].Use();
                 else
                     objectToSpawn[i].gameObject.SetActive(true);
             }
             if (!sentEvents[i])
             {
                 AppMetricaStartEvent(i);
             }
             currentIndex++;
         }
    }
    
    private void OnEnable()
    {
        playerExperience.ExperienceCounter.OnCountChangeEvent += CheckUnlock;
        foreach (var location in objectToSpawn)
        {
            if(location == null)
                continue;
            location.LocationBuy += Subscribe;
        }
    }

    private void OnDisable()
    {
        playerExperience.ExperienceCounter.OnCountChangeEvent -= CheckUnlock;
        foreach (var location in objectToSpawn)
        {
            if(location == null)
                continue;
            location.LocationBuy -= Subscribe;
        }
    }

    private void CheckUnlock()
    {
        for (int i = currentIndex; i < expCountToUnlock.Length; i++)
        {
            if (playerExperience.ExperienceCounter.Count >= expCountToUnlock[i])
            {
                currentIndex++;
                if (objectToSpawn[i] != null)
                {
                    objectToSpawn[i].gameObject.SetActive(true);
                }                    
                if(!sentEvents[i])
                {
                    AppMetricaStartEvent(i);
                }
                continue;
            }
            
            return;
        }
    }
    
    private void Subscribe(BuyLocation l)
    {
        for (int i = 0; i < objectToSpawn.Length; i++)
        {
            if (objectToSpawn[i] != null && l == objectToSpawn[i])
            {
                usedBuyLocations[i] = true;
                return;
            }
        }
    }

    public JToken CaptureAsJToken()
    {
        JToken[] array = new JToken[2];
        array[0] = JToken.FromObject(usedBuyLocations);
        array[1] = JToken.FromObject(sentEvents);
        return JToken.FromObject(array);
    }

    public void RestoreFromJToken(JToken state)
    {
        JToken[] array = state.ToObject<JToken[]>();
        usedBuyLocations = array[0].ToObject<bool[]>();
        sentEvents = array[1].ToObject<bool[]>();
    }
    
    private void AppMetricaStartEvent(int index)
    {
        if(index > 0)
            AppMetricaEndEvent(index - 1);
        sentEvents[index] = true;
        SendEvent("level_start", index);
    }

    private void AppMetricaEndEvent(int level_number)
    {
        SendEvent("level_finish", level_number);
    }

    private void SendEvent(string eventName, int level_number)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level_number", level_number);
        AppMetrica.Instance.ReportEvent(eventName, parameters);
        AppMetrica.Instance.SendEventsBuffer();
        GlobalEvents.InteractionFieldFinishFilling();
    }
}
