namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Helpers;
    using Interfaces;
    using Models;

    public class ResultTrimmer : IResultTrimmer
    {
        private readonly IMorpherCache _morpherCache;

        public ResultTrimmer(IMorpherCache morpherCache)
        {
            _morpherCache = morpherCache;
        }

        public void Trim(object obj, Guid? token)
        {
            if (token == null)
            {
                Trim(obj);
                return;
            }

            var cacheObject = (MorpherCacheObject) _morpherCache.Get(token.ToString().ToLowerInvariant());
            if (cacheObject.PaidUser)
            {
                return;
            }

            Trim(obj);
        }

        public void Trim(object obj)
        {
            if (obj == null)
            {
                return;
            }

             var props = obj
                .GetType()
                .GetProperties()
                .Where(info => info.GetCustomAttribute(typeof(OnlyForPayedAttribute)) != null)
                .ToArray();

            Trim(props, obj);

            var propsToGoIn = obj
                .GetType()
                .GetProperties()
                .Where(info => info.GetCustomAttribute(typeof(CheckForPayedAttribute)) != null)
                .ToArray();

            foreach (var info in propsToGoIn)
            {
                Trim(info.GetValue(obj));
            }
        }

        private void Trim(PropertyInfo[] props, object obj)
        {
            foreach (var prop in props)
            {
                var newValue = prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null;

                prop.SetValue(obj, newValue);
            }
        }
    }
}