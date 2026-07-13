using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace task07;

[AttributeUsage(AttributeTargets.Class,Inherited = true, AllowMultiple = false)]
public class VersionAttribute: Attribute
{
    public int Major {get;}
    public int Minor {get;}
    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}
