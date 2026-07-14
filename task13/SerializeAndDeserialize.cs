﻿using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace task13;
public class Subject
{
    public string Name { get; set; } = "";
    public int Grade { get; set; }
}
public class Student
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public List<Subject> Grades { get; set; } = new List<Subject>();
    public string? Information { get; set; }
}
public class CustomDateConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        string? datetime = reader.GetString();
        if (datetime == null)
        {
            throw new JsonException("Дата должна быть не null");
        }

        if (!DateTime.TryParseExact(datetime, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            throw new JsonException($"Неверный формат даты, верно: {DateFormat}");
        }
        return date;
    }
    public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}
public class JsonService
{
    private readonly JsonSerializerOptions options;
    public JsonService()
    {
        options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        };
        options.Converters.Add(new CustomDateConverter());
    }
    public string StudentSerialize(Student student)
    {
        return JsonSerializer.Serialize(student, options);
    }
    public Student StudentDeserialize(string json)
    {
        var student = JsonSerializer.Deserialize<Student>(json, options);
        if (student == null)
        {
            throw new ArgumentException("Возвращен null");
        }

        StudentValidate(student);
        return student;
    }
    public void SaveFile(Student student, string filepath)
    {
        File.WriteAllText(filepath, StudentSerialize(student));
    }
    public Student LoadFile(string filepath)
    {
        if (!File.Exists(filepath))
        {
            throw new FileNotFoundException("Файл не найден");
        }

        return StudentDeserialize(File.ReadAllText(filepath));
    }
    private void StudentValidate(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            throw new ArgumentException("Имя должно быть не пустым");
        }
        if (string.IsNullOrWhiteSpace(student.LastName))
        {
            throw new ArgumentException("Фамилия должна быть не пустой");
        }
        if (student.BirthDate > DateTime.Now)
        {
            throw new ArgumentException("Дата рождения должна быть в прошлом");
        }
        if (student.Grades == null || !student.Grades.Any())
        {
            throw new ArgumentException("Список оценок должен быть не пустым");
        }

        foreach (var subject in student.Grades)
        {
            if (string.IsNullOrWhiteSpace(subject.Name))
            {
                throw new ArgumentException("Название предмета должно быть не пустым");
            }
            if (subject.Grade < 2 || subject.Grade > 5)
            {
                throw new ArgumentException($"Оценка должна быть в диапазоне от 2 до 5");
            }
        }
    }
}
public class Program
{
    public static void Main()
    {
        try
        {
            var service = new JsonService();

            var student = new Student
            {
                FirstName = "Мария",
                LastName = "Бузмакова",
                BirthDate = new DateTime(2004, 12, 29),
                Grades = new List<Subject>
                    {
                        new Subject { Name = "Алгоритмизация и Программирование", Grade = 5 },
                        new Subject { Name = "Математический анализ", Grade = 4 },
                        new Subject { Name = "Математическая логика", Grade = 4 },
                        new Subject { Name = "Алгебра", Grade = 5 },
                        new Subject { Name = "Дискретная математика", Grade = 3 }
                    },
                Information = "Имеет социальную стипендию"
            };
            string jsonstudent = service.StudentSerialize(student);
            Console.WriteLine(jsonstudent);
            File.WriteAllText("student.json", jsonstudent);
            Console.WriteLine("Информация сохранена в файл 'student.json'");
            var load = service.LoadFile("student.json");
            Console.WriteLine($"Загружен студент: {load.FirstName} {load.LastName}");
            Console.WriteLine($"Дата рождения: {load.BirthDate:yyyy-MM-dd}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Ошибка {exception.GetType().Name}: {exception.Message}");
        }
        File.Delete("student.json");
    }
}
