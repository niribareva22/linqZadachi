using System;
using System.Linq;

int[] numbers = { 8, 3, 12, 5, 2, 9, 4, 7 };

Student[] students =
{
    new Student(1, "Ana", 17, "Burgas"),
    new Student(2, "Boris", 18, "Sofia"),
    new Student(3, "Clara", 20, "Burgas"),
    new Student(4, "Deyan", 19, "Varna"),
    new Student(5, "Elena", 18, "Sofia"),
    new Student(6, "Filip", 21, "Burgas")
};

Course[] courses =
{
    new Course(101, "C# Basics"),
    new Course(102, "Web Development"),
    new Course(103, "Databases"),
    new Course(104, "Algorithms")
};

Enrollment[] enrollments =
{
    new Enrollment(1, 1, 101, 5),
    new Enrollment(2, 2, 101, 6),
    new Enrollment(3, 3, 102, 6),
    new Enrollment(4, 4, 102, 4),
    new Enrollment(5, 5, 103, 5),
    new Enrollment(6, 6, 103, 6),
    new Enrollment(7, 2, 103, 4),
    new Enrollment(8, 3, 101, 3),
    new Enrollment(9, 6, 102, 5)
};

// 01. Square every number
var ex1 = numbers.Select(n => n * n);
Console.WriteLine(string.Join(", ", ex1));

// 02. Project student details
var ex2 = students.Select(s => new { s.Name, s.City });
Console.WriteLine(string.Join("; ", ex2.Select(s => $"{s.Name} / {s.City}")));

// 03. Keep the even numbers
var ex3 = numbers.Where(n => n % 2 == 0);
Console.WriteLine(string.Join(", ", ex3));

// 04. Filter by two conditions
var ex4 = students.Where(s => s.City == "Burgas" && s.Age >= 18);
Console.WriteLine(string.Join(" and ", ex4.Select(s => $"{s.Name} (Id {s.Id})")));

// 05. Names of adult students
var ex5 = students.Where(s => s.Age >= 18).Select(s => s.Name);
Console.WriteLine(string.Join(", ", ex5));

// 06. Sort the numbers
var ex6 = numbers.OrderBy(n => n);
Console.WriteLine(string.Join(", ", ex6));

// 07. Adults from youngest to oldest
var ex7 = students.Where(s => s.Age >= 18).OrderBy(s => s.Age).Select(s => s.Name);
Console.WriteLine(string.Join(", ", ex7));

// 08. Descending order with OrderBy
var ex8 = numbers.OrderBy(n => -n);
Console.WriteLine(string.Join(", ", ex8));

// 09. The first Sofia student
var ex9 = students.First(s => s.City == "Sofia");
Console.WriteLine($"{ex9.Name} (Id {ex9.Id})");
// Obqsnenie: Elena ne e vurnata, tui kato turseneto sledva posledovatelno redicata i purviqt otgovarqsht element v masiva e Boris.

// 10. When First cannot find a match
// Predskazanie: Shte se generira InvalidOperationException, tui kato redicata ne sudurja element, otgovarqsht na uslovieto.

// 11. Exactly one student
var ex11 = students.Single(s => s.Id == 4);
Console.WriteLine($"{ex11.Name} (Id {ex11.Id})");
// Obqsnenie: Metodut Single e podhodqsht, poneje unikalniqt identifikator (Id) garantira vrushtaneto na edinstven zapis.

// 12. Single with zero or several matches
// Predskazanie za Age == 18: InvalidOperationException poradi nalichieto na poveche ot edin suvdavashch element.
// Predskazanie za Id == 99: InvalidOperationException poradi lipsa na suvdavashchi elementi v redicata.

// 14. Group students by city
var ex14 = students.GroupBy(s => s.City);
foreach (var group in ex14)
{
    Console.WriteLine($"{group.Key}: {string.Join(", ", group.Select(s => s.Name))}");
}

// 15. Group minors and adults
var ex15 = students.GroupBy(s => s.Age >= 18);
foreach (var group in ex15)
{
    string label = group.Key ? "Adults" : "Minors";
    Console.WriteLine($"{label}: {string.Join(", ", group.Select(s => s.Name))}.");
}

// 16. High grades grouped by course
var ex16 = enrollments.Where(e => e.Grade >= 5).GroupBy(e => e.CourseId);
foreach (var group in ex16)
{
    Console.WriteLine($"{group.Key}: {string.Join(", ", group.Select(e => e.Id))}");
}

// 17. Attach student names to grades
var ex17 = enrollments.Join(students, e => e.StudentId, s => s.Id, (e, s) => new { e.Id, s.Name, e.Grade });
Console.WriteLine(string.Join("; ", ex17.Select(x => $"{x.Id}/{x.Name}/{x.Grade}")));

// 18. Courses and their high grades
var ex18 = courses.Join(enrollments, c => c.Id, e => e.CourseId, (c, e) => new { c.Title, e.StudentId, e.Grade })
                  .Where(x => x.Grade >= 5);
foreach (var item in ex18)
{
    Console.WriteLine($"{item.Title}: {item.StudentId}/{item.Grade}");
}
// Obqsnenie: Kursut Algorithms otsustva, tui kato nqma nalichni zapisani ocenki, otgovarqshti na uslovieto s ocenka >= 5.

// 19. A sorted report from three arrays
var ex19 = enrollments
    .Join(students, e => e.StudentId, s => s.Id, (e, s) => new { e.Id, s.Name, e.CourseId, e.Grade })
    .Join(courses, ec => ec.CourseId, c => c.Id, (ec, c) => new { ec.Id, ec.Name, CourseTitle = c.Title, ec.Grade })
    .Where(x => x.Grade >= 5)
    .OrderBy(x => x.Id)
    .Select(x => new { x.Name, x.CourseTitle, x.Grade });
Console.WriteLine(string.Join("; ", ex19.Select(x => $"{x.Name}/{x.CourseTitle}/{x.Grade}")));

// 20. The highest grade in each course
var ex20 = enrollments
    .Join(students, e => e.StudentId, s => s.Id, (e, s) => new { e, s })
    .Join(courses, es => es.e.CourseId, c => c.Id, (es, c) => new { CourseTitle = c.Title, StudentName = es.s.Name, es.e.Grade, CourseId = c.Id })
    .GroupBy(x => new { x.CourseId, x.CourseTitle })
    .OrderBy(g => g.Key.CourseId)
    .Select(g => g.OrderBy(x => -x.Grade).First())
    .Select(x => new { x.CourseTitle, x.StudentName, x.Grade });
Console.WriteLine(string.Join("; ", ex20.Select(x => $"{x.CourseTitle} / {x.StudentName} / {x.Grade}")));

public record Student(int Id, string Name, int Age, string City);
public record Course(int Id, string Title);
public record Enrollment(int Id, int StudentId, int CourseId, int Grade);