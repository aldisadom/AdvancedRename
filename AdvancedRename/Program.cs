using AdvancedRename.Models;
using AdvancedRename.Services;

namespace AdvancedRename;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Advanced rename started");

        string workDirectory = Directory.GetCurrentDirectory();
        ConfigurationService configuration = new(workDirectory);
        configuration.Read();
        ClearOutput(workDirectory);
        CheckInputFolders(workDirectory);
        CheckCFGFiles(workDirectory, configuration);

        if (configuration.Settings.Options == Options.Generate)
        {
            GeneratorService service = new(configuration, workDirectory);
            service.Generate();
        }
        else if (configuration.Settings.Options == Options.Rename)
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

    private static void CheckCFGFiles(string workDirectory, ConfigurationService configuration)
    {
        string file = workDirectory + "\\Map.csv";
        string data = string.Empty;
        string delimiter = configuration.Settings.DelimiterCSV;

        if (!File.Exists(file))
        {
            data = $"From{delimiter}To\r\n" +
                    $"Text to be replaced{delimiter}New text\r\n";
            File.WriteAllText(file, data);
        }

        file = workDirectory + "\\Generate.csv";

        if (!File.Exists(file))
        {
            data = $"Template{delimiter}Tag_to_replace{delimiter}Tag_to_replace2\r\n" +
                    $"#First column is template, other columns are for text to be replaced\r\n" +
                    $"VLVA{delimiter}LBG21AA151{delimiter}New text\r\n";
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
