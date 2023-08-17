using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "GSSC/DataSet/DefaultSet")]
public class DefaultSet : ScriptableObject
{
    [Header("Audio Group")]
    public AudioMixer MainMixer;
    public AudioMixerGroup Master;
    public AudioMixerGroup BGMMixer;
    public AudioMixerGroup SFXMixer;
    public AudioMixerGroup VoiceMixer;

}
