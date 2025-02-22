using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject projectileVFX;
    [SerializeField] private GameObject explosionVFX;

    private Unit target;

    private Coroutine flyCoroutine;

    private float threshold = 0.01f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float speed = 10f;
    private readonly static string explosionSoundAssetAddress = "SFX/Projectile_Hit.wav";
    private AudioClip explosionSound;
    private void Awake()
    {
        projectileVFX.SetActive(false);
        explosionVFX.SetActive(false);
    }

    private void Start()
    {
        explosionSound = AssetManager.Instance.GetAudio(explosionSoundAssetAddress);
        Debug.Assert(explosionSound);
    }

    public void Fire(Transform from, Unit to)
    {
        if(!to) return;
        if(flyCoroutine != null) return;
        transform.position = from.position;
        target = to;
        flyCoroutine = StartCoroutine(FlyToTarget());
    }

    private IEnumerator FlyToTarget()
    {
        explosionVFX.SetActive(false);
        projectileVFX.SetActive(true);
        while(target)
        {
            Vector3 diff = target.transform.position - transform.position;
            if(diff.magnitude < threshold) break;
            transform.position += diff.normalized * speed * Time.deltaTime;
            // fly
            yield return null;
        }
        projectileVFX.SetActive(false);
        flyCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!target) return;
        if(other.transform != target.transform) return;
            Explosion();
    }

    private void Explosion()
    {
        flyCoroutine = null;
        projectileVFX.SetActive(false);
        explosionVFX.SetActive(true);
        target.GetDamaged(damage);
        SoundManager.Instance.Play(explosionSound, SoundManager.sfxVolume);
        target = null;
    }
}
