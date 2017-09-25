namespace Morpher.WebService.V3.UnitTests
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Http.Dispatcher;
    using Russian;

    class TestWebApiResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies() => new List<Assembly>()
            { typeof(RussianAnalyzerController).Assembly };
    }
}
