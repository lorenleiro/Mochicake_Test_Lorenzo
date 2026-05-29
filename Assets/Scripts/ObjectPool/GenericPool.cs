using UnityEngine;
using UnityEngine.Pool;

public class GenericPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] 
    private T prefab;

    [SerializeField]
    private int maxPoolSize = 20;

    [SerializeField]
    private int defaultCapacity = 10;


    private ObjectPool<T> pool;
    private int spawnCount;

    protected virtual void Awake()
    {
        pool = new ObjectPool<T>(
            createFunc: () => Instantiate(prefab, transform),
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: obj => Destroy(obj.gameObject),
            defaultCapacity: defaultCapacity,
            maxSize: maxPoolSize
        );
    }

    public virtual T Get()
    {
        T obj = pool.Get();
        obj.name = $"{prefab.name}-{spawnCount}";
        spawnCount++;

        return obj;
    }

    public void Release(T obj)
    {
        pool.Release(obj);
        spawnCount--;
    }
}