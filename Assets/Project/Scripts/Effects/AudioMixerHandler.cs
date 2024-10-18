using UnityEngine.Audio;

public class AudioMixerHandler
{
    private AudioMixer mixer;

    private bool state = true;

    private const int OFFVALUE = -80;
    private const int ONVALUE = 0;

    public AudioMixerHandler(AudioMixer mixer)
    {
        this.mixer = mixer;
    }

    public bool GetState()
    {
        return state;
    }

    public void ChangeState()
    {
        state = !state;
        if(state) 
        {
            VolumeOn();
            return;
        }
        VolumeOff();
    }

    private void VolumeOn()
    {
        SetVolume(ONVALUE);
    }

    private void VolumeOff()
    {
        SetVolume(OFFVALUE);
    }

    private void SetVolume(int value)
    {
        mixer.SetFloat("MasterVolume", value);
    }    
}
