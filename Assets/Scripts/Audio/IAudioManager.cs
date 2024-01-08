
using UnityEngine;

public interface IAudioManager: IService
{
    public void PlayMusic(bool reset = false);
    public void StopMusic();
    public void PlayAudio(AudioClip clip);
}
