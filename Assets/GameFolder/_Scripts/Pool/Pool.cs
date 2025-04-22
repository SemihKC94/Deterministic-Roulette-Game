using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKC.DeterministicRoulette.Pool
{
    public abstract class Pool<T> : ScriptableObject where T : MonoBehaviour
    {
        [SerializeField] protected int MaxSize;
        [SerializeField] protected T Behaviour;

        readonly Queue<T> _pooledObjectQueue = new Queue<T>();
        PoolObject _poolInformer;

        public T TakeFromPool()
        {
            if (ReferenceEquals(_poolInformer, null))
            {
                GameObject go = new GameObject();
                _poolInformer = go.AddComponent<PoolObject>();
                _poolInformer.Destroyed += PoolInformerOnDestroyed;
            }

            T obj;
            if (_pooledObjectQueue.Count > 0)
            {
                obj = _pooledObjectQueue.Dequeue();
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = (T)Instantiate(Behaviour);
            }

            return obj;
        }

        public void PutBackToPool(T t)
        {
            if (_pooledObjectQueue.Count > MaxSize)
            {
                Destroy(t.gameObject);
            }
            else
            {
                t.gameObject.SetActive(false);
                _pooledObjectQueue.Enqueue(t);
            }
        }

        void PoolInformerOnDestroyed()
        {
            _poolInformer.Destroyed -= PoolInformerOnDestroyed;
            _poolInformer = null;
            _pooledObjectQueue.Clear();
        }
    }
}
