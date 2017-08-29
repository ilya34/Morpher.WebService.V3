namespace Morpher.WebService.V3.General.Data
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ThrottleThisAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LogThisAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class OnlyForPaidAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class CheckForPayedAttribute : Attribute
    {
    }
}