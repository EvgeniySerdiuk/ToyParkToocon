using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class GameButton : MonoBehaviour
{
    protected Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Perform);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Perform);
    }

    protected abstract void Perform();
}
