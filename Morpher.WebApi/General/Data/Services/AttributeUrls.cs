namespace Morpher.WebService.V3.General.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;

    public class AttributeUrls : IAttributeUrls
    {
        public HashSet<string> Urls { get; } = new HashSet<string>();

        public AttributeUrls(Type attributeType)
        {
            var apiType = typeof(ApiController);
            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(apiType));

            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods()
                    .Where(info => info.GetCustomAttribute(attributeType) != null);

                var controllerRoute = GetControllerRoute(controller);

                foreach (var methodInfo in methods)
                {
                    var methodRoute = GetActionRoute(methodInfo);
                    Urls.Add($"{controllerRoute.ToLowerInvariant()}/{methodRoute.ToLowerInvariant()}");
                }
            }
        }

        public static string GetControllerRoute(Type controller)
        {
            var routePrefixAttribute = controller.GetCustomAttribute<RoutePrefixAttribute>();
            var controllerRoute = routePrefixAttribute != null
                ? $"/{routePrefixAttribute.Prefix}" : $"/{controller.Name}";
            return controllerRoute;
        }

        public static string GetActionRoute(MethodInfo methodInfo)
        {
            var methodRouteAttribute = methodInfo.GetCustomAttribute<RoutePrefixAttribute>();
            var methodRoute = methodRouteAttribute != null
                ? methodRouteAttribute.Prefix
                : methodInfo.Name;
            return methodRoute;
        }

    }
}