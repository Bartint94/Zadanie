using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> Pools = new List<PooledObjectInfo>();

    GameObject _objectPoolHolder;
    static GameObject _gameObjectsHolder;
    public static int spawnedShootings;
    private void Awake()
    {
        SetupEmptyHolder();
    }
    void SetupEmptyHolder()
    {
        _objectPoolHolder = new GameObject("Pooled Objects");

        _gameObjectsHolder = new GameObject("Game Objects");
        _gameObjectsHolder.transform.SetParent(_objectPoolHolder.transform);

    }
    public static GameObject Spawn(GameObject spawnObject, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = Pools.Find(p => p.objectName == spawnObject.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { objectName = spawnObject.name };
            Pools.Add(pool);
        }

        GameObject spawnableObject  = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(spawnObject, spawnPosition, spawnRotation);
          
            if (_gameObjectsHolder != null)
            {
                spawnableObject.transform.SetParent(_gameObjectsHolder.transform);
            }
        }
        else
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }
        return spawnableObject;
    }
    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);

        PooledObjectInfo pool = Pools.Find(pool => pool.objectName == goName);

        if(pool == null)
        {
            Debug.Log($"object: {obj.name} in not pooled");
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}
public class PooledObjectInfo
{
    public string objectName;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
