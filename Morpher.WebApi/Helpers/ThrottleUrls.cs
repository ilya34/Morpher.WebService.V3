namespace Morpher.WebService.V3.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;

    public static class ThrottleUrls
    {
        public static readonly HashSet<string> Urls = new HashSet<string>();

        static ThrottleUrls()
        {
            var apiType = typeof(ApiController);
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(apiType));

            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods()
                    .Where(info => info.GetCustomAttribute<ThrottleThisAttribute>() != null);

                var routePrefixAttribute = controller.GetCustomAttribute<RoutePrefixAttribute>();
                var controllerRoute = routePrefixAttribute != null 
                    ? $"/{routePrefixAttribute.Prefix}" : $"/{controller.Name}";

                foreach (var methodInfo in methods)
                {
                    var methodRouteAttribute = methodInfo.GetCustomAttribute<RoutePrefixAttribute>();
                    var methodRoute = methodRouteAttribute != null
                        ? methodRouteAttribute.Prefix
                        : methodInfo.Name;

                    Urls.Add($"{controllerRoute.ToLowerInvariant()}/{methodRoute.ToLowerInvariant()}");
                }
            }
        }
    }
}