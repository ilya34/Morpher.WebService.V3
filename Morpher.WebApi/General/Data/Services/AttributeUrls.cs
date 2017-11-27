namespace Morpher.WebService.V3.General.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;

    public class AttributeUrls : IAttributeUrls
    {
        public Dictionary<string, ThrottleThisAttribute> Urls { get; } = new Dictionary<string, ThrottleThisAttribute>();

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
                    var attribute = methodInfo.GetCustomAttribute<ThrottleThisAttribute>();
                    Urls.Add($"{controllerRoute.ToLowerInvariant()}/{methodRoute.ToLowerInvariant()}", attribute);
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
            var methodRouteAttribute = methodInfo.GetCustomAttribute<RouteAttribute>();
            var methodRoute = methodRouteAttribute != null
                ? methodRouteAttribute.Template
                : methodInfo.Name;
            return methodRoute;
        }

    }
}