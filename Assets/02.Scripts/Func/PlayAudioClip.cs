using UnityEngine;

public class PlayAudioClip : MonoBehaviour
{
    [Range(0,1f)]
    public float Volume = 1f;

    public AudioClip Clip;
    public SoundManager.SoundType soundType = SoundManager.SoundType.SFX;

    public void Call()
    {
        Managers.SoundManager.SoundInvoke(Clip, Volume, soundType);
    }
}
