namespace Morpher.WebService.V3.UnitTests
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Http.Dispatcher;

    class TestWebApiResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies() => new List<Assembly>()
            { typeof(Morpher.WebService.V3.Controllers.RussianAnalyzerController).Assembly };
    }
}
