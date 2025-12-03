using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic object pool for reusing component instances to improve performance by avoiding frequent instantiation and destruction.
/// </summary>
/// <typeparam name="T">The type of Component to pool.</typeparam>
public class ObjectPool<T> where T : Component
{
    private Queue<T> _pool = new Queue<T>();
    private List<T> _activePool = new List<T>();
    private T _prefab;
    private Transform _parent;

    /// <summary>
    /// Gets a read-only list of currently active objects in the pool.
    /// </summary>
    public IReadOnlyList<T> ActiveObjects => _activePool.AsReadOnly();

    /// <summary>
    /// Initializes the pool with a prefab and prewarms it with a specified number of instances.
    /// </summary>
    /// <param name="prefab">The prefab to use for instantiating new objects.</param>
    /// <param name="initialSize">The number of instances to prewarm the pool with.</param>
    /// <param name="parent">The parent transform to assign to pooled objects.</param>
    public ObjectPool(T prefab, int initialSize, Transform parent)
    {
        _parent = parent;
        _prefab = prefab;
        Prewarm(initialSize, parent);
    }

    /// <summary>
    /// Prewarms the pool by instantiating and storing inactive instances of the prefab.
    /// </summary>
    /// <param name="count">The number of objects to prewarm.</param>
    /// <param name="parent">The parent transform to assign to the objects.</param>
    private void Prewarm(int count, Transform parent)
    {
        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(_prefab, parent);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Retrieves an object from the pool, sets its position and parent, and activates it.
    /// </summary>
    /// <param name="position">The position to place the object at.</param>
    /// <param name="parent">The new parent for the object.</param>
    /// <returns>The activated object.</returns>
    public T Get(Vector3 position, Transform parent)
    {
        T obj = _pool.Count > 0 ? _pool.Dequeue() : GameObject.Instantiate(_prefab, parent);
        obj.transform.position = position;
        obj.transform.SetParent(parent);
        obj.gameObject.SetActive(true);
        _activePool.Add(obj);
        return obj;
    }

    /// <summary>
    /// Returns an object back to the pool, deactivates it, and resets its parent.
    /// </summary>
    /// <param name="obj">The object to return to the pool.</param>
    public void ReturnToPool(T obj)
    {
        if (obj == null || obj.Equals(null)) return;

        if (obj.gameObject != null)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_parent);
            _activePool.Remove(obj);
            _pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Returns all active objects back to the pool and deactivates them.
    /// </summary>
    public void ReturnAllToPool()
    {
        for (int i = _activePool.Count - 1; i >= 0; i--)
        {
            ReturnToPool(_activePool[i]);
        }

        _activePool.Clear();
    }

    /// <summary>
    /// Destroys all pooled and active objects and clears the pool.
    /// </summary>
    public void Clear()
    {
        while (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            GameObject.Destroy(obj.gameObject);
        }

        for (int i = _activePool.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(_activePool[i].gameObject);
        }

        _activePool.Clear();
    }
}
