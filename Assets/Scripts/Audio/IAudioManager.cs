
using UnityEngine;

public interface IAudioManager: IService
{
    public void PlayMusic(bool reset = false);
    public void StopMusic();
    public void PlaySoundEffect(AudioClip clip);
    public void SetMusicVolume(float volume);
    public void SetSoundEffectsVolume(float volume);
}
