using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace task07;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class DisplayNameAttribute: Attribute
{
    public string DisplayName {get;}
    public DisplayNameAttribute(string displayname)
    {
        DisplayName = displayname;
    }
}
