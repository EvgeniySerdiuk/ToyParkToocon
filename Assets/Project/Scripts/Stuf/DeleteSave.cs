using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class DeleteSave : MonoBehaviour
{
    private SavingWrapper wrapper;

    [Inject]
    private void Init(SavingWrapper wrapper)
    {
        this.wrapper = wrapper;
    }

    [Button]
    private void Delete()
    {
        wrapper.ClearList();
        wrapper.DeleteSaveFile();
    }
}
