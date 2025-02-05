using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource bgmSource;
    private List<SoundObject> availableSoundPool = new List<SoundObject>();
    private List<SoundObject> playingSoundPool = new List<SoundObject>();
    [SerializeField] private int maxPoolSize;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        bgmSource = GetComponent<AudioSource>();
        Debug.Assert(bgmSource);
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
    }

    private void Start()
    {
        for(int i = 0 ; i < maxPoolSize; ++i)
            CreateSoundObject();
    }

    private SoundObject CreateSoundObject()
    {
        var go = new GameObject(){name = $"SoundSource{availableSoundPool.Count}"};
        go.transform.parent = transform;
        var SoundObject = go.AddComponent<SoundObject>();
        availableSoundPool.Add(SoundObject);
        go.SetActive(false);
        return SoundObject;
    }

    private SoundObject GetSoundObject()
    {
        if(availableSoundPool.Count == 0)
            return null;
        var soundObject = availableSoundPool.Last();
        availableSoundPool.RemoveAt(availableSoundPool.Count - 1);
        return soundObject;
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        var soundObject = GetSoundObject();
        if(soundObject == null) return;
        soundObject.gameObject.SetActive(true);
        playingSoundPool.Add(soundObject);
        soundObject.Play(clip, volume);
    }
    public void ReturnSoundObject(SoundObject soundObject)
    {
        if(!playingSoundPool.Contains(soundObject)) return;
        playingSoundPool.Remove(soundObject);
        availableSoundPool.Add(soundObject);
    }
    
}
