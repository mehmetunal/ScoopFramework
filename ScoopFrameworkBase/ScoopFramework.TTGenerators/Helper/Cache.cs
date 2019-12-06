using System;
using System.Collections.Generic;
using System.Linq;

namespace ScoopFramework.TTGenerators.Helper
{
    public class Cache<TKey, TValue>
    {

        Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
        public IEnumerable<TValue> Values { get { return dic.Values; } }
        public IEnumerable<TKey> Keys { get { return dic.Keys; } }
        public int Size { get; private set; }
        public Cache(int size)
        {
            Size = size;
        }
        public bool TryGet(TKey key, out TValue value)
        {
            return dic.TryGetValue(key, out value);
        }
        public void Remove(TKey key)
        {
            if (dic.ContainsKey(key))
                dic.Remove(key);
        }
        public void Add(TKey key, TValue value)
        {
            try
            {


                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, value);

                    int size = dic.Count - Size;
                    dic.Keys.Take(size).ToList().ForEach(a => dic.Remove(a));
                }
                else
                    dic[key] = value;
            }
            catch (Exception)
            {


            }
        }
        public TValue Get(TKey key, Func<TValue> create)
        {
            TValue val;
            if (!dic.TryGetValue(key, out val))
            {
                val = create();
                Add(key, val);
            }
            return val;
        }


    }
}
