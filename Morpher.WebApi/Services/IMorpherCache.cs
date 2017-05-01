namespace Morpher.WebApi.Services
{
    using System;

    public interface IMorpherCache
    {
        bool Decrement(string key);

        object Get(string key, string regionName = null);

        void Set(string key, object value, DateTimeOffset absoluteExpirationDateTimeOffset, string regionName = null);

        object Remove(string key, string regionName = null);
    }
}