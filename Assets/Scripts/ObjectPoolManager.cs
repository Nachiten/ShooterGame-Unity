using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

public enum ObjectType
{
    EnemyNormal,
    EnemyFast,
    EnemySlow,
    Ammo
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    
    public GameObject enemySlowPrefab;
    private ObjectPool<GameObject> enemySlowPool;
    
    public GameObject enemyNormalPrefab;
    private ObjectPool<GameObject> enemyNormalPool;
    
    public GameObject enemyFastPrefab;
    private ObjectPool<GameObject> enemyFastPool;

    public GameObject ammoPrefab;
    private ObjectPool<GameObject> ammoPool;

    private GameObject ammoParent, enemyParent;
    
    private const int defaultPoolSize = 20;
    private const int maxPoolSize = 50;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        Assert.IsNotNull(enemySlowPrefab);
        Assert.IsNotNull(enemyNormalPrefab);
        Assert.IsNotNull(enemyFastPrefab);
        Assert.IsNotNull(ammoPrefab);
        
        enemyParent = GameObject.Find("Enemies");
        ammoParent = GameObject.Find("Ammo");
    }
   
    private void Start()
    {
        enemySlowPool = new ObjectPool<GameObject>(() =>   Instantiate(enemySlowPrefab, enemyParent.transform), defaultCapacity:defaultPoolSize, maxSize:maxPoolSize);
        enemyNormalPool = new ObjectPool<GameObject>(() => Instantiate(enemyNormalPrefab, enemyParent.transform), defaultCapacity:defaultPoolSize, maxSize:maxPoolSize);
        enemyFastPool = new ObjectPool<GameObject>(() =>   Instantiate(enemyFastPrefab, enemyParent.transform), defaultCapacity:defaultPoolSize, maxSize:maxPoolSize);
        
        ammoPool = new ObjectPool<GameObject>(() => Instantiate(ammoPrefab, ammoParent.transform), defaultCapacity:defaultPoolSize, maxSize:maxPoolSize);
    }

    public GameObject getObject(ObjectType type)
    {
        return type switch
        {
            ObjectType.EnemyNormal => enemyNormalPool.Get(),
            ObjectType.EnemyFast => enemyFastPool.Get(),
            ObjectType.EnemySlow => enemySlowPool.Get(),
            ObjectType.Ammo => ammoPool.Get(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public void returnObject(GameObject obj, ObjectType type)
    {
        obj.SetActive(false);
        
        switch (type)
        {
            case ObjectType.EnemyNormal:
                enemyNormalPool.Release(obj);
                break;
            case ObjectType.EnemyFast:
                enemyFastPool.Release(obj);
                break;
            case ObjectType.EnemySlow:
                enemySlowPool.Release(obj);
                break;
            case ObjectType.Ammo:
                ammoPool.Release(obj);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
