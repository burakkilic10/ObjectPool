using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Object pool.
/// </summary>

[System.Serializable]
public class ObjectPool
{
    [SerializeField]
    private Dictionary<string, PooledObject> objectMap;

    [SerializeField]
    private static ObjectPool instance;

    [SerializeField]
    private Transform parent;

    int i = 0;

    private ObjectPool()
    {
    }

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <returns>The instance.</returns>
    public static ObjectPool GetInstance()
    {
        if (instance == null)
            instance = new ObjectPool();

        return instance;
    }

    /// <summary>
    /// Init the Object pool under the transform which holds the pooled objects.
    /// </summary>
    /// <param name="tr">Parent game object transform.</param>
    public void Init()
    {
        GameObject objectPool = new GameObject("ObjectPool");
        Object.DontDestroyOnLoad(objectPool);

        objectMap = new Dictionary<string, PooledObject>();
        parent = objectPool.transform;
    }

    /// <summary>
    /// Registers the object.
    /// </summary>
    /// <param name="prefabPath">Prefab path.</param>
    /// <param name="key">Key.</param>
    /// <param name="numberOfInstances">Number of instances.</param>
    public void RegisterObject(string prefabPath, string key, int numberOfInstances = 0)
    {
        if (!objectMap.ContainsKey(key))
        {
            if (numberOfInstances == 0)
            {
                objectMap.Add(key, new PooledObject(key, prefabPath, parent));
            }
            else
            {
                objectMap.Add(key, new PooledObject(key, prefabPath, numberOfInstances, parent));
            }
        }
    }

    /// <summary>
    /// Unregisters the object.
    /// </summary>
    /// <param name="key">Key.</param>
    public void UnregisterObject(string key)
    {
        if (!objectMap.ContainsKey(key))
        {
            objectMap.Remove(key);
        }
    }

    /// <summary>
    /// Returns the pooled object.
    /// Key is obtained from <code>PooledObjectMonoExtension</code> component attached the game object
    /// </summary>
    /// <param name="go">Go.</param>
    // TODO: You should be returning object to the pool after your job is finished with that object.
    public void ReturnObject(GameObject go)
    {
        if (go != null && go.GetComponent<PooledObjectMonoExtension>() != null)
        {
            ReturnObject(go, go.GetComponent<PooledObjectMonoExtension>().GetKey());
        }
    }


    /// <summary>
    /// Gets the pooled object.
    /// </summary>
    /// <returns>The object.</returns>
    /// <param name="key">Key.</param>
    public GameObject GetObject(string key)
    {
        PooledObject p = objectMap[key];
        return p.GetObjectInstance();
    }

    /// <summary>
    /// Returns the pooled object.
    /// </summary>
    /// <param name="go">Go.</param>
    /// <param name="key">Key.</param>
    // TODO: You should be returning object to the pool after your job is finished with that object.
    public void ReturnObject(GameObject go, string key)
    {
        go.transform.SetParent(parent);
        go.SetActive(false);
        objectMap[key].ReturnObjectInstance(go);
    }
}


