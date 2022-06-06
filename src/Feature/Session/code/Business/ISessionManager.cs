// Decompiled with JetBrains decompiler
// Type: Saudia.Booking.Business.ISessionManager
// Assembly: Saudia.Booking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 57295FBE-7636-4E01-8835-1C0E611E19F5
// Assembly location: D:\Projects\SaudiA\Saudia.Booking.dll

namespace SaudiA.Foundation.Session.Business
{
  public interface ISessionManager
  {
    T Get<T>(string key);

    void Save<T>(string key, T entity);

    void Remove(string key);

    void RemoveAll();
  }
}
