using System;

namespace ASPLAB2.API.Middleware
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiKeyAuthorizationAttribute : Attribute
    {
    }
}
