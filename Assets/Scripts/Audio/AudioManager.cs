using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioManager
{
    [SerializeField] private AudioSource _musicSrc;
    [SerializeField] private AudioSource _sfxSrc;


    public void Initialize() { }


    public void PlayMusic(bool reset = false)
    {
        if(reset) _musicSrc.Stop();
        _musicSrc.Play();
    }

    public void StopMusic()
    {
        _musicSrc.Stop();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        _sfxSrc.PlayOneShot(clip);
    }

    public void SetMusicVolume(float volume)
    {
        _musicSrc.volume = Mathf.Clamp01(volume);
    }

    public void SetSoundEffectsVolume(float volume)
    {
        _sfxSrc.volume = Mathf.Clamp01(volume);
    }

    public void DestroyAudioManager() => Destroy(gameObject);

}
