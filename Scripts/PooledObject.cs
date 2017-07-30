
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Wrapper for pooled objects
/// </summary>
public class PooledObject
{
    private readonly int OBJECT_POOL__DEFAUL_INITIAL_POOL_SIZE = 5;
    private readonly int OBJECT_POOL__DEFAUL_EXTEND_SIZE = 1;
    private List<GameObject> gameObjectList;
    private string key;
    private string prefabPath;
    private bool extendWhenExhaust = false;
    private int poolSize;
    private Transform parent;
    private GameObject prefab;

    protected static object _lock = new object();

    public PooledObject(string key, string prefabPath, int poolSize, Transform parent)
    {
        this.key = key;
        this.prefabPath = prefabPath;
        this.poolSize = poolSize;
        this.parent = parent;
        Init();
    }

    public PooledObject(string key, string prefabPath, Transform parent)
    {
        this.key = key;
        this.prefabPath = prefabPath;
        this.poolSize = OBJECT_POOL__DEFAUL_INITIAL_POOL_SIZE;
        this.parent = parent;
        Init();
    }

    private PooledObject Init()
    {
        prefab = Resources.Load<GameObject>(prefabPath);
        gameObjectList = new List<GameObject>();

        if (key.Length > 0 && prefabPath.Length > 0)
        {
            for (int i = 0; i < poolSize; i++)
            {
                CreateGameObject();
            }
        }
        return this;
    }

    void CreateGameObject()
    {
        GameObject go = null;

        try
        {
            go = (GameObject)Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(parent);
            go.SetActive(false);
            go.AddComponent<PooledObjectMonoExtension>().SetKey(key);
        }
        catch (UnityException e)
        {
            Debug.Log(e);
        }
        if (go != null)
        {
            gameObjectList.Add(go);
        }
    }

    public GameObject GetObjectInstance()
    {
        GameObject go = null;
        if (gameObjectList != null)
        {
            if (gameObjectList.Count > 0)
            {
                lock (_lock)
                {
                    go = gameObjectList[gameObjectList.Count - 1];
                    gameObjectList.RemoveAt(gameObjectList.Count - 1);
                }
            }
            else
            {
                for (int i = 0; i < OBJECT_POOL__DEFAUL_EXTEND_SIZE; i++)
                {
                    CreateGameObject();
                    go = gameObjectList[gameObjectList.Count - 1];
                    gameObjectList.RemoveAt(gameObjectList.Count - 1);
                }
            }

        }
        return go;
    }

    public void ReturnObjectInstance(GameObject go)
    {

        if (go != null)
        {
            if (gameObjectList == null)
            {
                gameObjectList = new List<GameObject>();
            }

            Transform tr = go.transform;
            tr.position = Vector3.zero;
            tr.localScale = Vector3.one;
            tr.eulerAngles = Vector3.zero;

            gameObjectList.Add(go);
        }
    }
}

