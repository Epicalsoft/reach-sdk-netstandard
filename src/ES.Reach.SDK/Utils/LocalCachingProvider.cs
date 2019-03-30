using Akavache;
using Newtonsoft.Json;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Epicalsoft.Reach.Api.Client.Net.Utils
{
    internal class LocalCachingProvider
    {
        public static LocalCachingProvider Instance = new LocalCachingProvider();

        static LocalCachingProvider()
        {
            BlobCache.ApplicationName = "Epicalsoft.Reach.Api.Client.Net";
            Registrations.Start("Epicalsoft.Reach.Api.Client.Net");
        }

        public static void ClearLocalCache()
        {
            BlobCache.LocalMachine.InvalidateAll();
        }

        private LocalCachingProvider()
        {
        }

        public void SaveState<T>(string key, T value)
        {
            BlobCache.LocalMachine.InsertObject(key, JsonConvert.SerializeObject(value));
        }

        public async Task<T> LoadState<T>(string key, T def = default(T))
        {
            try
            {
                string value = await BlobCache.LocalMachine.GetObject<string>(key);
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }

    public class CachingObject<T>
    {
        public CachingObject(T state)
        {
            State = state;
            ExpirationDate = DateTime.UtcNow.AddDays(7.0);
        }

        public DateTime ExpirationDate { get; set; }
        public T State { get; set; }

        public bool IsExpired
        {
            get
            {
                return DateTime.UtcNow > ExpirationDate;
            }
        }
    }
}