using UnityEngine;
using System.Collections;

public class PooledObjectMonoExtension : MonoBehaviour
{
    [SerializeField]
    public string objectKey;

    public void SetKey(string key)
    {
        objectKey = key;
    }

    public string GetKey()
    {
        return objectKey;
    }
}