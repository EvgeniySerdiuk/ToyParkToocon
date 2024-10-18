using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractionAudioPlayer : MonoBehaviour
{
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        if (source.isPlaying || source == null) 
            return;
        source.PlayOneShot(clip);
    }

    public void PlayAudio(AudioClip clip, float volume)
    {
        if (source.isPlaying || source == null)
            return;
        source.PlayOneShot(clip,volume);
    }
}
