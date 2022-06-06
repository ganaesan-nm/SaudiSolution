// Decompiled with JetBrains decompiler
// Type: Saudia.Booking.Business.SessionManager
// Assembly: Saudia.Booking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 57295FBE-7636-4E01-8835-1C0E611E19F5
// Assembly location: D:\Projects\SaudiA\Saudia.Booking.dll

using System.Web;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace SaudiA.Foundation.Session.Business
{
    public class SessionManager : ISessionManager
    {
        public T Get<T>(string key)
        {
            if (HttpContext.Current?.Session == null)
                return default;
            var obj = HttpContext.Current?.Session[key];
            if (obj == null)
                return default;
            return typeof(T) != typeof(string) ? JsonConvert.DeserializeObject<T>(obj.ToString()) : (T) obj;
        }

        public void Save<T>(string key, T entity)
        {
            if (HttpContext.Current?.Session != null)
            {
                if (typeof(T) == typeof(string) || entity == null)
                    HttpContext.Current.Session[key] = entity;
                else
                    HttpContext.Current.Session[key] = (object) JsonConvert.SerializeObject((object) entity);
            }
            else
            {
                Log.Info("http context is null", this);
            }
        }

        public void Remove(string key)
        {
            if (HttpContext.Current?.Session == null)
                return;
            HttpContext.Current.Session.Remove(key);
        }

        public void RemoveAll()
        {
            if (HttpContext.Current?.Session == null)
                return;
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}