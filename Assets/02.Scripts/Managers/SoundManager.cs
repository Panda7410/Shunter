using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    Action BGMVolAction;
    public enum SoundType { BGM, SFX , Voice};

    public AudioMixer MainMixer;
    public AudioMixerGroup Master;
    public AudioMixerGroup BGMMixer;
    public AudioMixerGroup SFXMixer;
    public AudioMixerGroup VoiceMixer;


    public float MasterSoundVol = 1f;
    public float BGMSoundVol = 1f;
    public float SFXSoundVol = 1f;
    public float VoiceSoundVol = 1f;


    AudioSource audioSourceBGM;
    AudioSource audioSourceVoice;




    void Awake()
    {
        DefaultSet @default = Managers.Resource.Load<DefaultSet>("Data/DefaultSet");
        MainMixer = @default.MainMixer;
        Master = @default.Master;
        BGMMixer = @default.BGMMixer;
        SFXMixer = @default.SFXMixer;
        VoiceMixer = @default.VoiceMixer;

        if (audioSourceBGM == null) //AddBGM
        {
            audioSourceBGM = this.gameObject.AddComponent<AudioSource>();
            audioSourceBGM.outputAudioMixerGroup = BGMMixer;
        }
        if (audioSourceVoice == null) //AddBGM
        {
            audioSourceVoice = this.gameObject.AddComponent<AudioSource>();
            audioSourceVoice.outputAudioMixerGroup = VoiceMixer;
        }
    }

    /// <summary>
    /// InstanceSoundPlay
    /// </summary>
    /// <param name="clip">audioSource to Play</param>
    /// <param name="soundType">audioSource type. SFX or BGM</param>
    public AudioSource SoundInvoke(AudioClip clip, SoundType soundType = SoundType.SFX)
    {
        if (audioSourceBGM == null) //AddBGM
        {
            audioSourceBGM = this.gameObject.AddComponent<AudioSource>();
            audioSourceBGM.outputAudioMixerGroup = BGMMixer;
        }
        AudioSource audioSource = null;

        if (soundType == SoundType.SFX)
        {
            GameObject soundObj = new GameObject(clip.name);
            audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = SFXMixer;
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(soundDestroyDelay(audioSource));
        }
        else if (soundType == SoundType.BGM)
        {
            audioSource = audioSourceBGM;
            audioSource.outputAudioMixerGroup = BGMMixer;

            StartCoroutine(BGM_Changer(audioSource, clip));
        }
        else if(soundType == SoundType.Voice)
        {
            audioSource = audioSourceVoice;
            audioSource.outputAudioMixerGroup = VoiceMixer;

            StartCoroutine(Voice_Changer(audioSource, clip));
        }

        return audioSource;
    }



    /// <summary>
    /// InstanceSoundPlay
    /// </summary>
    /// <param name="clip">audioSource to Play</param>
    /// <param name="Vol">Volume of audioSource. 0.0 to 1</param>
    /// <param name="soundType">audioSource type. SFX or BGM</param>
    public AudioSource SoundInvoke(AudioClip clip, float Vol, SoundType soundType = SoundType.SFX)
    {
        AudioSource audioSource = SoundInvoke(clip, soundType);

        switch (soundType)
        {
            case SoundType.BGM:
                BGMVolAction += () => {
                    audioSource.volume = Vol;
                    BGMVolAction = null;
                };
                break;
            case SoundType.SFX:
                audioSource.volume = Vol;
                break;
            case SoundType.Voice:
                audioSource.volume = Vol;
                break;
            default:
                break;
        }

        return audioSource;
    }
    IEnumerator soundDestroyDelay(AudioSource audioSource)
    {
        float waitTime = audioSource.clip.length;
        yield return new WaitForSeconds(waitTime);
        if (audioSource != null)
        {
            GameObject @object = audioSource.transform.gameObject;
            Destroy(@object);
        }
    }

    IEnumerator BGM_Changer(AudioSource audioSource, AudioClip clip)
    {
        if (audioSource.clip != null && audioSource.isPlaying && audioSource.clip != clip)
            while (audioSource.volume > 0.1)
            {
                audioSource.volume -= Time.deltaTime * 0.7f;
                yield return null;
            }
        if (audioSource.clip != clip)
            audioSource.clip = clip;
        audioSource.loop = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
        if (clip == null)
        {
            audioSource.Stop();
        }
        audioSource.volume = 1;
        yield return null;
        if (BGMVolAction != null)
            BGMVolAction.Invoke();
    }

    IEnumerator Voice_Changer(AudioSource audioSource, AudioClip clip)
    {

        if (audioSource.clip != clip)
            audioSource.clip = clip;
        if (clip == null)
        {
            audioSource.Stop();
            yield break;
        }
        audioSource.Play();
        yield return null;
    }


    public void AudioSoundControll(float SoundVol, string MixName)
    {
        float vol = SoundVol;
        if (vol == -40f) vol = -80f;
        try
        { MainMixer.SetFloat(MixName, vol); }
        catch
        { Debug.LogError("믹서에 파라미터 할당이 되어있지 않습니다."); }
    }
}
