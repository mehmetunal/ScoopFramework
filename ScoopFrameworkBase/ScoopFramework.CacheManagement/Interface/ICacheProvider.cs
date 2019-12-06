namespace ScoopFramework.CacheManagement.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheProvider<T>
    {
        /// <summary>
        /// Cache Object Remove
        /// </summary>
        /// <param name="cacheName"></param>
        void CacheRemove(string cacheName);

        /// <summary>
        /// Cache Object Remove
        /// </summary>
        void CacheRemoveAll();

        /// <summary>
        /// Cache Object Remove
        /// </summary>
        /// <param name="cacheNameList"></param>
        void CacheRemoveCustomList(List<string> cacheNameList);

        /// <summary>
        /// Cache Object Add
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cacheName"></param>
        /// <param name="expiration"></param>
        void CacheAdd(T model, string cacheName, DateTime expiration);

        /// <summary>
        /// Cache Add Method, With CacheEntryRemovedCallback
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cacheName"></param>
        /// <param name="expiration"></param>
        /// <param name="method"></param>
        void CacheAdd(T model, string cacheName, DateTime expiration, CacheEntryRemovedCallback method);

        /// <summary>
        /// Cache is Null Control
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        bool CacheCheck(string cacheName);


        /// <summary>
        /// Cache is Return List
        /// </summary>
        /// <returns></returns>
        List<string> CacheList();

        /// <summary>
        /// Cache Object Get
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        T CacheGet(string cacheName);
    }
}
