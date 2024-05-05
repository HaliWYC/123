using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Database")]
    public SoundDetailsList_SO SoundDetailsData;
    public SceneSoundList_SO SceneSoundData;

    [Header("Audio Source")]
    public AudioSource gameSource;
    public AudioSource ambientSource;

    public Coroutine soundRoutine;

    [Header("AudioMixer")]
    public AudioMixer audioMixer;

    [Header("Snapshots")]
    public AudioMixerSnapshot normalSnapshots;
    public AudioMixerSnapshot ambientSnapshots;
    public AudioMixerSnapshot muteSnapshots;


    public float musicStartSecond => Random.Range(5f, 15f);

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.EndGameEvent += OnEndGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.EndGameEvent -= OnEndGameEvent;
    }

    private void OnEndGameEvent()
    {
        if (soundRoutine != null)
            StopCoroutine(soundRoutine);
        muteSnapshots.TransitionTo(1f);
    }

    private void OnAfterSceneLoadedEvent()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        SceneSoundDetails sceneSound = SceneSoundData.GetSceneSoundDetails(currentScene);
        if (sceneSound == null)
            return;
        
        SoundDetails gameSound = SoundDetailsData.GetSoundDetails(sceneSound.gameSound);
        SoundDetails ambientSound = SoundDetailsData.GetSoundDetails(sceneSound.ambientSound);

        if (soundRoutine != null)
            StopCoroutine(soundRoutine);
        soundRoutine = StartCoroutine(PlaySoundRoutine(gameSound, ambientSound));
    }

    private IEnumerator PlaySoundRoutine(SoundDetails game, SoundDetails ambient)
    {
        if(game!=null && ambient != null)
        {
            PlayAmbientSound(ambient, 1f);
            yield return new WaitForSeconds(musicStartSecond);
            PlayGameSound(game,musicStartSecond);
        }
    }


    /// <summary>
    /// Play game background music
    /// </summary>
    /// <param name="soundClip"></param>
    private void PlayGameSound(SoundDetails soundClip, float transitionTime)
    {
        audioMixer.SetFloat("GameVolume", ConvertSoundToVolume(soundClip.soundVolume));
        gameSource.clip = soundClip.soundClip;
        if (gameSource.isActiveAndEnabled)
            gameSource.Play();
        normalSnapshots.TransitionTo(transitionTime);
    }

    /// <summary>
    /// Play ambient music
    /// </summary>
    /// <param name="soundClip"></param>
    private void PlayAmbientSound(SoundDetails soundClip, float transitionTime)
    {
        audioMixer.SetFloat("AmbientVolume", ConvertSoundToVolume(soundClip.soundVolume));
        ambientSource.clip = soundClip.soundClip;
        if (ambientSource.isActiveAndEnabled)
            ambientSource.Play();
        ambientSnapshots.TransitionTo(transitionTime);
    }

    private float ConvertSoundToVolume(float amount)
    {
        return (amount * 100 - 80);
    }

    public void SetMaterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", value * 100 - 80);
    }
}
