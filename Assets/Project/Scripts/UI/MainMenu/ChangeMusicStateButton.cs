using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChangeMusicStateButton : GameButton
{
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    private AudioMixerHandler mixer;
    private Image musicImage;

    [Inject]
    private void Init(AudioMixerHandler mixer)
    {
        this.mixer = mixer;
        musicImage = GetComponent<Image>();
        ConfigureState();
    }

    private void ConfigureState()
    {
        Sprite currentSprite = mixer.GetState() ? musicOnSprite : musicOffSprite;
        musicImage.sprite = currentSprite;
    }

    protected override void Perform()
    {
        mixer.ChangeState();
        ConfigureState();
    }
}
