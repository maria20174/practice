using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using task13;
namespace task13tests;

public class StudentJsonTests
{
    private readonly JsonService service;

    public StudentJsonTests()
    {
        service = new JsonService();
    }

    [Fact]
    public void Serialize_ReturnsCorrectJson()
    {
        var student = new Student
        {
            FirstName = "Дмитрий",
            LastName = "Нагиев",
            BirthDate = new DateTime(1990, 03, 15),
            Grades = new List<Subject>
            {
                new Subject { Name = "Математика", Grade = 3 },
                new Subject { Name = "Русский язык", Grade = 5 }
            },
            Information = null
        };
        string json = service.StudentSerialize(student);
        Assert.Contains("Дмитрий", json);
        Assert.Contains("Нагиев", json);
        Assert.Contains("Математика", json);
        Assert.Contains("Русский язык", json);
        Assert.Contains("\"BirthDate\": \"1990-03-15\"", json);
        Assert.DoesNotContain("Information", json);
    }

    [Fact]
    public void Serialize_WithInformation()
    {
        var student = new Student
        {
            FirstName = "Виктор",
            LastName = "Баринов",
            BirthDate = new DateTime(2005, 11, 30),
            Grades = new List<Subject> 
            { 
                new Subject { Name = "Русский язык", Grade = 5 },
                new Subject { Name = "Биология", Grade = 5 },
                new Subject { Name = "Химия", Grade = 5 }

            },
            Information = "Отличник"
        };
        string json = service.StudentSerialize(student);
        Assert.Contains("Виктор", json);
        Assert.Contains("Баринов", json);
        Assert.Contains("Русский язык", json);
        Assert.Contains("Биология", json);
        Assert.Contains("Химия", json);
        Assert.Contains("\"BirthDate\": \"2005-11-30\"", json);
        Assert.Contains("Отличник", json);
    }

    [Fact]
    public void Deserialize_ValidJson_ReturnsStudent()
    {
        string json = @"{
                ""FirstName"": ""Роман"",
                ""LastName"": ""Пуртов"",
                ""BirthDate"": ""2004-01-27"",
                ""Grades"": 
                [
                { ""Name"": ""Информатика"", ""Grade"": 5 },
                { ""Name"": ""Алгебра"", ""Grade"": 4 },
                { ""Name"": ""Физика"", ""Grade"": 4 }
                ]
            }";
        var student = service.StudentDeserialize(json);
        Assert.Equal("Роман", student.FirstName);
        Assert.Equal("Пуртов", student.LastName);
        Assert.Equal(new DateTime(2004, 01, 27), student.BirthDate);
        Assert.Equal(3, student.Grades.Count);
    }

    [Fact]
    public void Deserialize_InvalidDateFormat()
    {
        string json = @"{
                ""FirstName"": ""Оксана"",
                ""LastName"": ""Миронова"",
                ""BirthDate"": ""21-06-2010"",
                ""Grades"": 
                [
                { ""Name"": ""Литература"", ""Grade"": 5 },
                { ""Name"": ""Обществознание"", ""Grade"": 4 }
                ]
            }";
        var exception = Assert.Throws<JsonException>(() => service.StudentDeserialize(json));
        Assert.Contains("Неверный формат даты, верно:", exception.Message);
    }

    [Fact]
    public void Deserialize_EmptyFirstName()
    {
        string json = @"{
                ""FirstName"": """",
                ""LastName"": ""Жижерунова"",
                ""BirthDate"": ""2006-07-29"",
                ""Grades"": 
                [
                { ""Name"": ""Математика"", ""Grade"": 4 },
                { ""Name"": ""Русский"", ""Grade"": 3 }
                ]
            }";
        var exception = Assert.Throws<ArgumentException>(() => service.StudentDeserialize(json));
        Assert.Contains("Имя", exception.Message);
    }

    [Fact]
    public void Deserialize_WithInformation()
    {
        string json = @"{
                ""FirstName"": ""Руслан"",
                ""LastName"": ""Русланов"",
                ""BirthDate"": ""2007-04-05"",
                ""Grades"": 
                [
                { ""Name"": ""География"", ""Grade"": 2 },
                { ""Name"": ""История"", ""Grade"": 3 }
                ],
                ""Information"": ""Отправлен на пересдачу""
            }";
        var student = service.StudentDeserialize(json);
        Assert.Equal(2, student.Grades[0].Grade);
        Assert.Equal("Отправлен на пересдачу", student.Information);
    }
    [Fact]
    public void Program_Run_PrintsStudentInfo()
    {
        var output = new StringWriter();
        Console.SetOut(output);
        task13.Program.Main();
        var service = new JsonService();
        Assert.Contains("Мария Бузмакова", output.ToString());
        Assert.Contains("2004-12-29", output.ToString());
        Assert.Contains("Алгоритмизация и программирование", output.ToString());
        Assert.Contains("Математический анализ", output.ToString());
        Assert.Contains("Математическая логика", output.ToString());
        Assert.Contains("Алгебра", output.ToString());
        Assert.Contains("Дискретная математика", output.ToString());
        Assert.Contains("Имеет социальную стипендию", output.ToString());
        Assert.Contains("Информация сохранена в файл", output.ToString());
        Assert.Contains("Загружен студент: Мария Бузмакова", output.ToString());
        Assert.Contains("Дата рождения: 2004-12-29", output.ToString());
        Assert.Contains("\"FirstName\"", output.ToString());
        Assert.Contains("\"LastName\"", output.ToString());
        Assert.Contains("\"BirthDate\"", output.ToString());
        Assert.Contains("\"Grades\"", output.ToString());
        Assert.Contains("\"Information\"", output.ToString());
    }


}
