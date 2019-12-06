using System.Collections.Generic;
using System.Linq;

namespace ScoopFramework.TTGenerators.Helper
{
    public class MRUCache<T, KEY> where T : CacheItem<KEY>
    {
        public MRUCache(int size = 2000)
        {
            MaxSize = size;
        }
        Dictionary<KEY, LinkedListNode<T>> _index = new Dictionary<KEY, LinkedListNode<T>>();

        protected LinkedList<T> _data = new LinkedList<T>();


        public long TotalSize { get; private set; }

        public IEnumerable<T> GetCacheItems()
        {
            return _data.Select(a => a);
        }
        protected T GetCacheItem(KEY key)
        {
            lock (this)
            {
                LinkedListNode<T> node;
                if (_index.TryGetValue(key, out node))
                {

                    _data.Remove(node);
                    _data.AddFirst(node);
                    return node.Value;
                }
            }
            var ret = LoadCacheItem(key);
            if (ret != null)
            {
                AddCacheItem(ret);
            }
            return ret;

        }
        virtual protected T LoadCacheItem(KEY key)
        {
            return default(T);
        }

        public long MaxSize { get; set; }
        protected void AddCacheItem(T item)
        {
            lock (this)
            {
                if (_data.Count > MaxSize)
                {
                    var l = _data.Last;
                    if (l != null)
                    {
                        if (OnRemoveCacheItem(l.Value))
                        {
                            Remove(l.Value.Key);
                        }
                    }
                }

                TotalSize += item.Size;

                _index[item.Key] = _data.AddFirst(item);
            }
        }
        protected void Remove(KEY key)
        {
            lock (this)
            {
                LinkedListNode<T> node;
                if (_index.TryGetValue(key, out node))
                {
                    TotalSize -= node.Value.Size;
                    _index.Remove(key);
                    _data.Remove(node);
                }
            }
        }
        protected virtual bool OnRemoveCacheItem(T item)
        {
            return true;
        }
        protected void Clear()
        {
            foreach (var p in _data)
            {
                OnRemoveCacheItem(p);
            }
            _data.Clear();
            _index.Clear();
        }
    }
    public interface CacheItem<KEY>
    {
        KEY Key { get; }
        int Size { get; }
    }
}
