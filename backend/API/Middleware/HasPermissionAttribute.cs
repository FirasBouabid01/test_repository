using System;

namespace API.Middleware;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class HasPermissionAttribute : Attribute
{
    public string Permission { get; }

    public HasPermissionAttribute(string permission)
    {
        Permission = permission;
    }
}
