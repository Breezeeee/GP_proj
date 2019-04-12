using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//// Object Pool for tiles
public class TilePool : UnitySingleton<TilePool>
{
    public GameObject[] tilePrefabs;
    public int poolSize = 8;
    public bool lockPoolSize = true;
    [HideInInspector]
    public List<List<GameObject>> pooledObjs;

    void Start(){
        //// initial objs in pool
        pooledObjs = new List<List<GameObject>>();

        for (int prefabIndex = 0; prefabIndex < tilePrefabs.Length; prefabIndex++)
        {
            pooledObjs.Add(new List<GameObject>());
            for (int i = 0; i < poolSize; i++)
            {
                GameObject go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
                go.SetActive(false);
                go.transform.SetParent(transform);
                pooledObjs[prefabIndex].Add(go);
            }
        }
    }

    //// get a obj from the pool
    public GameObject GetPooledObject(int index)
    {
        for (int i = 0; i < pooledObjs[index].Count; ++i)
        {
            if(!pooledObjs[index][i].activeInHierarchy){
                return pooledObjs[index][i];
            }
        }

        if(!lockPoolSize)
        {
            GameObject obj = Instantiate(tilePrefabs[index]);
            pooledObjs[index].Add(obj);
            return obj;
        }
        
        print("No enough objs in pool!\n");
        return null;
    }
    
}
