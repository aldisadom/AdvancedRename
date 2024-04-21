using AdvancedRename.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdvancedRename.Services;

public class ConfigurationService

{
    public ConfigurationSettings Settings { get; set; } = new();

    private readonly string _fileName;

    public ConfigurationService(string workDirectory)
    {
        _fileName = workDirectory + "\\Configuration.json";
    }

    public void Read()
    {
        if (!File.Exists(_fileName))
            Write();

        string[] textLines = File.ReadAllLines(_fileName);
        string text = string.Empty;

        //removing comments 
        foreach (string line in textLines)
        {
            if (line.StartsWith("#"))
                continue;

            text += line;
        }

        try
        {
            Settings = JsonConvert.DeserializeObject<ConfigurationSettings>(text, new StringEnumConverter())!;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to read configuration settings");
        }

        Write();
    }

    public void Write()
    {
        string text = Settings.AddComments();
        text += JsonConvert.SerializeObject(Settings, Formatting.Indented, new StringEnumConverter());
        File.WriteAllText(_fileName, text);
    }
}
