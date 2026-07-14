using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace task07;

public static class ReflectionHelper 
{
    public static void PrintTypeInfo(Type type)
    {
        var Name = type.GetCustomAttribute<DisplayNameAttribute>();
        if (Name != null)
        {
            Console.WriteLine($"{type.Name} - {Name.DisplayName}");
        }
        var Vers = type.GetCustomAttribute<VersionAttribute>();
        if (Vers != null)
        {
            Console.WriteLine($"Version: {Vers.Major}.{Vers.Minor}");
        }
        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(a => a.GetCustomAttribute<DisplayNameAttribute>() != null).ToList();
        Console.WriteLine("Methods:");
        if (methods.Count() != 0)
        {
            foreach (var method in methods)
            {
                Console.WriteLine($"{method.Name} - {method.GetCustomAttribute<DisplayNameAttribute>()!.DisplayName}");
            }
        
        }
        else
        {
            Console.WriteLine("No methods");
        }
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly).Where(a => a.GetCustomAttribute<DisplayNameAttribute>() != null).ToList();
        Console.WriteLine("Properties:");
        if (properties.Count() != 0)
        {
            foreach (var property in properties)
            {
                Console.WriteLine($"{property.Name}, {property.PropertyType.Name} - {property.GetCustomAttribute<DisplayNameAttribute>()!.DisplayName}");
            }
        }
        else
        {
            Console.WriteLine("No properties");
        }
    } 
}
