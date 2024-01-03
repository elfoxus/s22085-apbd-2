using apbd_2_s22085.DTO;

namespace apbd_2_s22085;

public class CSVParser
{
    private CSVParser()
    {
    }

    public static List<Line> Parse(string path)
    {
        using var reader = new StreamReader(path);
        var lines = new List<Line>();
        
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var columns = line.Split(',');
            lines.Add(new Line(line, columns.ToList()));
        }
        
        return lines;
    }
}