using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceKit.Common.Extensions;
using NServiceKit.Redis;

namespace CacheUtil
{
    public class Caching
    {
        private static Caching _instancia;
        public static Caching Instancia
        {
            get {
                if (_instancia == null)
                    _instancia = new Caching();
                return _instancia; 
            }
        }

        public bool Existe(string key)
        {
            using (var redisClient = new RedisClient())
            {
                return redisClient.Exists(key) > 0;
            }
        }

        public void Remover(string key)
        {
            using (var redisClient = new RedisClient())
            {
                redisClient.Remove(key);
            }
        }
        public void RemoverTodos()
        {
            using (var redisClient = new RedisClient())
            {
                redisClient.FlushAll();
            }
        }

        public T Retornar<T>(string key)
        {
            try
            {
                using (var redisClient = new RedisClient())
                {
                    return redisClient.Get<T>(key);
                }
            }
            catch { return default(T); }
        }
        
        public void Definir<T>(string key, T value)
        {
            Definir(key, value, new TimeSpan(1, 0, 0));
        }
        public void Definir<T>(string key, T value, TimeSpan expiredIn)
        {
            try
            {
                using (var redisClient = new RedisClient())
                {
                    var redis = redisClient.Set<T>(key, value, expiredIn);
                }
            }
            catch { }
        }

        public void Definir(string key, dynamic value)
        {
            Definir(key, value, new TimeSpan(0, 2, 0));
        }
        public void Definir(string key, dynamic value, TimeSpan expiredIn)
        {
            try
            {
                using (var redisClient = new RedisClient())
                {
                    var redis = redisClient.Set<dynamic>(key, value, expiredIn);
                }
            }
            catch { }
        }

        public object Retornar(string key)
        {
            try
            {
                using (var redisClient = new RedisClient())
                {
                    return redisClient.Get<dynamic>(key);
                }
            }
            catch { return null; }
        }
    }
}
