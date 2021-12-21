using System;
using UnityEngine;
using UnityEngine.Pool;

public enum EnemyType
{
    Normal,
    Fast,
    Slow
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

    private const int defaultPoolSize = 20;
    private const int maxPoolSize = 50;
    
    private void Awake()
    {
        instance = this;

        enemySlowPool = new ObjectPool<GameObject>(() =>   Instantiate(enemySlowPrefab), defaultCapacity:defaultPoolSize, maxSize:maxPoolSize);
        enemyNormalPool = new ObjectPool<GameObject>(() => Instantiate(enemyNormalPrefab), defaultCapacity:defaultPoolSize, maxSize:maxPoolSize);
        enemyFastPool = new ObjectPool<GameObject>(() =>   Instantiate(enemyFastPrefab), defaultCapacity:defaultPoolSize, maxSize:maxPoolSize);
        
        ammoPool = new ObjectPool<GameObject>(() => Instantiate(ammoPrefab), defaultCapacity:defaultPoolSize, maxSize:maxPoolSize);
    }

    public GameObject getObject(EnemyType type)
    {
        return type switch
        {
            EnemyType.Normal => enemyNormalPool.Get(),
            EnemyType.Fast => enemyFastPool.Get(),
            EnemyType.Slow => enemySlowPool.Get(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public void returnObject(GameObject obj, EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Normal:
                enemyNormalPool.Release(obj);
                break;
            case EnemyType.Fast:
                enemyFastPool.Release(obj);
                break;
            case EnemyType.Slow:
                enemySlowPool.Release(obj);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
