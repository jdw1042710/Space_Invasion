using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance;

    private string[] assetAdressList = 
    {
        "VFX/Projectile_01.prefab",
        "VFX/Projectile_02.prefab",
    };
    
    private Dictionary<string, GameObject> objectPool = new Dictionary<string, GameObject>();
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
        foreach(string address in assetAdressList)
        {
            Addressables.LoadAssetAsync<GameObject>(address).Completed += (handle) => 
            {
                if(handle.Status != AsyncOperationStatus.Succeeded)
                    return;
                //
                objectPool.Add(address, handle.Result);
            };
        }
    }

    public GameObject GetObject(string address)
    {
        if(!objectPool.ContainsKey(address))
            return null;
        return objectPool[address];
    }
}
