using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace task07;

[DisplayName("Пример класса")]
[Version(1,0)]
public class SampleClass
{
   [DisplayName("Тестовый метод")]
   public void TestMethod() {}
   [DisplayName("Числовое свойство")]
   public int Number {get; set;}
}
