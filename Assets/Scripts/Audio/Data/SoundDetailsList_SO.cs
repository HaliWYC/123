using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW SoundDetailsList",menuName ="Sound/SoundDetailsList")]
public class SoundDetailsList_SO : ScriptableObject
{
    public List<SoundDetails> soundDetailsList;

    public SoundDetails GetSoundDetails(string name)
    {
        return soundDetailsList.Find(s => s.soundName == name);
    }
}

[System.Serializable]
public class SoundDetails
{
    public string soundName;
    public AudioClip soundClip;
    [Range(0.1f, 1.5f)]
    public float soundMinPitch;
    [Range(0.1f, 1.5f)]
    public float soundMaxPitch;
    [Range(0.1f, 1f)]
    public float soundVolume;
}
