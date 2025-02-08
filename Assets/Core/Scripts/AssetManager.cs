using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance;

    private string[] prefabAssetAddressList = 
    {
        "VFX/Projectile_01.prefab",
        "VFX/Projectile_02.prefab",
    };

    private string[] audioAssetAddressList = 
    {
        "SFX/Projectile_Hit.wav",
        "SFX/Melee_Hit.wav",
        "SFX/Projectile_Fire.wav",
    };
    
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, AudioClip> audios = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadAssets();
    }

    private void LoadAssets()
    {
        foreach(string address in prefabAssetAddressList)
        {
            prefabs.Add(address, LoadAsset<GameObject>(address));
        }
        foreach(string address in audioAssetAddressList)
        {
            audios.Add(address, LoadAsset<AudioClip>(address));
        }
    }

    private T LoadAsset<T>(string address) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(address);
        if(handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.Log($"Load {address} is failed");
            return null;
        }
        //
        handle.WaitForCompletion();
        return handle.Result;
    }

    public GameObject GetPrefab(string address)
    {
        if(!prefabs.ContainsKey(address))
            prefabs.Add(address, LoadAsset<GameObject>(address));
        return prefabs[address];
    }

    public AudioClip GetAudio(string address)
    {
        if(!audios.ContainsKey(address))
        {
            audios.Add(address, LoadAsset<AudioClip>(address));
        }
        return audios[address];
    }
}
