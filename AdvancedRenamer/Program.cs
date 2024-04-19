using AdvancedRenamer.Models;
using AdvancedRenamer.Services;

namespace AdvancedRenamer;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Advanced rename started");

        string workDirectory = Directory.GetCurrentDirectory();
        Configuration configuration = new(workDirectory);
        configuration.Read();
        ClearOutput(workDirectory);
        CheckInputFolders(workDirectory);
        CheckCFGFiles(workDirectory, configuration);

        if (configuration.Generate)
        {
            GeneratorService service = new(configuration, workDirectory);
            service.Generate();
        }
        else
        {
            RenameService service = new(configuration, workDirectory);
            service.Rename();
        }

        Console.WriteLine("Advanced rename finished");
    }

    private static void CheckInputFolders(string workDirectory)
    {
        string directory = workDirectory + "\\input";

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        directory = workDirectory + "\\templates";

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }

    private static void CheckCFGFiles(string workDirectory, Configuration configuration)
    {
        string file = workDirectory + "\\Map.csv";
        string data = string.Empty;

        if (!File.Exists(file))
        {
            data = $"From{configuration.DelimiterCSV}To\r\n" +
                    $"Text to be replaced{configuration.DelimiterCSV}New text\r\n";
            File.WriteAllText(file, data);
        }

        file = workDirectory + "\\Generate.csv";
        
        if (!File.Exists(file))
        {
            data = $"Template{configuration.DelimiterCSV}Tag_to_replace{configuration.DelimiterCSV}Tag_to_replace2\r\n" +
                    $"#First column is template, other columns are for text to be replaced\r\n" +
                    $"VLVA{configuration.DelimiterCSV}LBG21AA151{configuration.DelimiterCSV}New text\r\n";
            File.WriteAllText(file, data);
        }
    }

    private static void ClearOutput(string workDirectory)
    {
        string directory = workDirectory + "\\output";

        if (Directory.Exists(directory))
            Directory.Delete(directory, true);

        Directory.CreateDirectory(directory);
    }
}
