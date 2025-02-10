using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = transform.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        SoundManager.Instance.ReturnSoundObject(this);
        gameObject.SetActive(false);
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
