using AzureWebApp.Models;
using AzureWebApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureWebApp.Managers
{
    public class CacheManager
    {

        private static List<Fruit> GetFruitListFromDB()
        {
            var list = new List<Fruit>();
            //Pretending we are getting from database
            list.Add(new Fruit { FruitID = 1, Color = "Yellow", FruitName = "Banana", EntryTime = DateTime.Now.ToLongTimeString() });
            list.Add(new Fruit { FruitID = 2, Color = "Purple", FruitName = "Grape", EntryTime = DateTime.Now.ToLongTimeString() });
            list.Add(new Fruit { FruitID = 3, Color = "Red", FruitName = "Apple", EntryTime = DateTime.Now.ToLongTimeString() });
            list.Add(new Fruit { FruitID = 1, Color = "Orange", FruitName = "Orange", EntryTime = DateTime.Now.ToLongTimeString() });


            return list;
        }


        /// <summary>
        /// Getting a Fruit List from the Redis Cache
        /// </summary>
        /// <param name="IsRefreshCache">Optional to get fresh data from the datasource</param>
        /// <returns></returns>
        public static List<Fruit> GetFruitListFromCache(bool? IsRefreshCache = false)
        {
            bool IsRefresh = Convert.ToBoolean(IsRefreshCache);
            string cachename = "Common.FruitList";

            var list = new List<Fruit>();
            list.Add(new Fruit()
            {Color = "BLue",
            EntryTime = DateTime.Now.ToLongTimeString(),
            FruitName = "Blue Bary"
            });
            try
            {
                var cache = new CacheService();

                if (cache.IsAlive())
                {
                    if (cache.Exists(cachename))
                    {

                        if (IsRefresh)
                        {
                            list = GetFruitListFromDB();
                            if (list.Count > 0)
                            {
                                SaveToCache(cache, cachename, list);
                            }
                        }
                        else
                        {
                            list = RetrieveCacheByCacheName<Fruit>(cachename, cache);
                        }
                    }
                    else
                    {
                        list = GetFruitListFromDB();
                        if (list.Count > 0)
                        {
                            SaveToCache(cache, cachename, list);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                //TO DO if cache failed
                Console.Write(ex.Message);
            }

            return list;
        }

        public static string DeleteFruitList()
        {
            string cachename = "Common.FruitList";
            try
            {
                var cache = new CacheService();

                if (cache.IsAlive())
                {
                    if (cache.Exists(cachename))
                    {
                        cache.Remove(cachename);
                    }

                    #region purpose: quick checking if the cache is deleted
                    if (cache.Exists(cachename))
                    {
                        return "still exist";
                        //Console.Write("still exist");
                    }
                    else
                    {
                        return "deleted";
                        //Console.Write("deleted");
                    }
                    #endregion
                }

                return "Not IsAlive";



            }
            catch (Exception ex)
            {

                //TO DO if delete failed
                Console.Write(ex.Message);
                return ex.Message;
            }

        }


        #region cache operations

        private static List<T> RetrieveCacheByCacheName<T>(string cachename, CacheService cache)
        {
            var genList = new List<T>();

            if (cache.IsAlive())
            {
                if (cache.Exists(cachename))
                {
                    var deserializeObj = cache.Get(cachename);

                    genList = JsonConvert.DeserializeObject<List<T>>(deserializeObj);
                }
            }
            return genList;
        }

        private static void SaveToCache<T>(CacheService cache, string cachename, T t)
        {
            string serializeObj = Serialize(t);
            cache.Save(cachename, serializeObj, new TimeSpan(0, 0, 30));
            //cache.Save(cachename, serializeObj, new TimeSpan(23, 55, 0));
        }


        #endregion

        #region supports

        private static string Serialize(object o)
        {
            string s = string.Empty;

            s = JsonConvert.SerializeObject(o);

            return s;
        }

        #endregion
    }
}
