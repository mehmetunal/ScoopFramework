using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using ScoopFramework.CacheManagement.Interface;

namespace ScoopFramework.CacheManagement.Implementation
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheProvider<T> : ICacheProvider<T>
    {
        readonly ObjectCache _cache;
        readonly CacheItemPolicy _cacheItemPolicy;

        /// <summary>
        /// CacheProvider 
        /// </summary>
        public CacheProvider()
        {
            _cache = MemoryCache.Default;
            _cacheItemPolicy = new CacheItemPolicy();
        }

        /// <summary>
        /// Cache Insert Method, With CacheEntryRemovedCallback 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cacheName"></param>
        /// <param name="expiration"></param>
        /// <param name="method"></param>
        public void CacheAdd(T model, string cacheName, DateTime expiration, CacheEntryRemovedCallback method)
        {
            _cacheItemPolicy.AbsoluteExpiration = expiration;
            _cacheItemPolicy.RemovedCallback = method;
            _cache.Add(cacheName, model, _cacheItemPolicy);
        }

        /// <summary>
        /// Cache List Insert Method, With CacheEntryRemovedCallback 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cacheName"></param>
        /// <param name="expiration"></param>
        /// <param name="method"></param>

        public void CacheAdd(List<T> model, string cacheName, DateTime expiration, CacheEntryRemovedCallback method)
        {
            _cacheItemPolicy.AbsoluteExpiration = expiration;
            _cacheItemPolicy.RemovedCallback = method;
            _cache.Add(cacheName, model, _cacheItemPolicy);
        }


        /// <summary>
        /// Cache Insert Method
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cacheName"></param>
        /// <param name="expiration"></param>
        public void CacheAdd(T model, string cacheName, DateTime expiration)
        {
            _cacheItemPolicy.AbsoluteExpiration = expiration;
            _cache.Add(cacheName, model, _cacheItemPolicy);
        }

        /// <summary>
        /// Cache List Insert Method
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cacheName"></param>
        /// <param name="expiration"></param>
        public void CacheAdd(List<T> model, string cacheName, DateTime expiration)
        {
            _cacheItemPolicy.AbsoluteExpiration = expiration;
            _cache.Add(cacheName, model, _cacheItemPolicy);
        }

        /// <summary>
        /// Cache Object is Null Control Method
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public bool CacheCheck(string cacheName)
        {
            return _cache.Contains(cacheName);
        }

        /// <summary>
        /// Cache Object Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public T CacheGet(string cacheName)
        {
            return (T)_cache.Get(cacheName);
        }

        /// <summary>
        /// MEMORY CLEAR
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cache Object Remove
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public void CacheRemove(string cacheName)
        {
            MemoryCache.Default.Remove(cacheName);
        }

        /// <summary>
        /// Cache Object Remove All
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void CacheRemoveAll()
        {
            var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (var cacheKey in cacheKeys)
            {
                MemoryCache.Default.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Cache Object Remove All Cache List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void CacheRemoveCustomList(List<string> cacheNameList)
        {
            var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (var cacheKey in cacheKeys.Where(cacheNameList.Contains))
            {
                MemoryCache.Default.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Cache Object List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<string> CacheList()
        {
            return MemoryCache.Default.Select(kvp => kvp.Key).OrderBy(x => x).ToList();
        }
    }
}