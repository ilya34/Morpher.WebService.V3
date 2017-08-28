namespace Morpher.WebService.V3.Helpers
{
    using App_Start;
    using FluentScheduler;

    public class JobFactory : IJobFactory
    {
        public IJob GetJobInstance<T>() where T : IJob
        {
            return (IJob)AutofacInit.AutofacWebApiDependencyResolver.GetService(typeof(T));
        }
    }
}