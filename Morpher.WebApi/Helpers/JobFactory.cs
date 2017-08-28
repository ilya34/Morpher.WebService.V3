namespace Morpher.WebService.V3.Helpers
{
    using System.Web.Mvc;
    using App_Start;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;
    using FluentScheduler;

    public class JobFactory : IJobFactory
    {
        public IJob GetJobInstance<T>() where T : IJob
        {
            return (IJob)AutofacInit.AutofacWebApiDependencyResolver.GetService(typeof(T));
        }
    }
}