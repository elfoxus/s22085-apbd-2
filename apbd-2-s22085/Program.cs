
using System.Text.Json;
using apbd_2_s22085.DTO;
using apbd_2_s22085.Model;

namespace apbd_2_s22085;

public class Program
{
    public static void Main(string[] args)
    {
        var (csvPath, resultDir, logsPath, exportFormat) = ValidateInput(args);

        var lines = CSVParser.Parse(csvPath);
        var logs = new List<string>();
        
        var students = extractStudents(lines, logs);
        var activeStudies = extractActiveStudies(students);
        var university = new University(DateTime.Today.ToString("dd.MM.yyyy"), "Jan Kowalski", students, activeStudies);
        
        saveResult(exportFormat, university, resultDir);
        File.WriteAllLines(logsPath, logs);
    }

    private static void saveResult(string exportFormat, University university, string resultDir)
    {
        var result = new Result(university);
        switch (exportFormat)
        {
            case "json":
                exportToJson(result, resultDir);
                break;
            default:
                throw new InvalidOperationException("Invalid export format");
        }
    }

    private static void exportToJson(Result result, string resultDir)
    {
        var json = JsonSerializer.Serialize(result);
        File.WriteAllText(Path.Join(resultDir, "uczelnia.json"), json);
    }
    
    private static List<ActiveStudy> extractActiveStudies(HashSet<Student> students)
    {
        var activeStudies = new List<ActiveStudy>();
        var activeStudiesDict = new Dictionary<string, int>();
        foreach (var student in students)
        {
            if (activeStudiesDict.ContainsKey(student.studies.name))
            {
                activeStudiesDict[student.studies.name]++;
            }
            else
            {
                activeStudiesDict.Add(student.studies.name, 1);
            }
        }
        
        foreach (var (name, numberOfStudents) in activeStudiesDict)
        {
            // according to the example, the number of students should be a string in resulting JSON
            activeStudies.Add(new ActiveStudy(name, numberOfStudents.ToString())); 
        }

        return activeStudies;
    }

    private static HashSet<Student> extractStudents(List<Line> lines, List<string> logs)
    {
        var students = new HashSet<Student>(new StudentEqualityComparer());
        foreach (var (line, columns) in lines)
        {
            if (columns.Count != 9)
            {
                logs.Add($"Wiersz nie posiada odpowiedniej ilości kolumn: {line}");
                continue;
            }

            if (columns.Exists(col => string.IsNullOrEmpty(col)))
            {
                logs.Add($"Wiersz nie może posiadać pustych kolumn: {line}");
                continue;
            }

            var date = DateOnly.Parse(columns[5]).ToString();

            var student = new Student($"s{columns[0]}", // according to the example, the index should be prefixed with 's'
                columns[0],
                columns[1],
                date,
                columns[6],
                columns[7],
                columns[8],
                new Studies(columns[2], columns[3]));

            if (!students.Add(student))
            {
                logs.Add($"Duplikat: {line}");
            }
        }

        return students;
    }

    private static (string, string, string, string) ValidateInput(string[] args)
    {
        
        if (args.Length < 4 || args.Length > 4)
        {
            throw new ArgumentOutOfRangeException();
        }
        
        var csvPath = args[0];
        var resultDir = args[1];
        var logsPath = args[2];
        var exportFormat = args[3];
        
        if (!File.Exists(csvPath))
        {
            throw new FileNotFoundException("CSV file not found");
        }

        if (!Directory.Exists(resultDir))
        {
            throw new DirectoryNotFoundException("Result directory not found");
        }
        
        if (exportFormat != "json")
        {
            throw new InvalidOperationException("Invalid export format");
        }

        if (!File.Exists(logsPath))
        {
            throw new FileNotFoundException("Logs file not found at the specified path");
        }
        
        return (csvPath, resultDir, logsPath, exportFormat);
    }
}

