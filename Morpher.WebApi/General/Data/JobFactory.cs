﻿namespace Morpher.WebService.V3.General
{
    using FluentScheduler;

    public class JobFactory : IJobFactory
    {
        public IJob GetJobInstance<T>() where T : IJob
        {
            return (IJob)AutofacInit.AutofacWebApiDependencyResolver.GetService(typeof(T));
        }
    }
}