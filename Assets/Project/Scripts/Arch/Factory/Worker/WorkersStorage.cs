using NaughtyAttributes;
using Project.Scripts.Arch.Workers;
using UnityEngine;

namespace Project.Scripts.Arch.Factory.Worker
{
    [CreateAssetMenu(fileName = "WorkerStorage", menuName = "Storage/WorkerStorage", order = 0)]
    public class WorkersStorage : ScriptableObject
    {
        private const string SheetId = "1xCLjT0V_BobAjtWjvHPEpdl48l1BtftEta3q16nA8Gw";

        public BaseWorker[] Storage;
        public Characteristics[] Characteristics;

        [Button]
        public void DownloadCharacteristics()
        {
            foreach (var Char in Characteristics)
            {
                Char.DownloadData(SheetId);
                ReadGoogleSheets.SetDirty(this);
            }
        }
    }
}