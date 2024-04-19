using Newtonsoft.Json;

namespace AdvancedRenamer.Models;

public class Configuration
{
    public bool Generate { get; set; } = false;
    public bool Rename { get; set; } = true;
    public string DelimiterCSV { get; set; } = ",";

    private readonly string _fileName;

    public Configuration(string workDirectory)
    {
        _fileName = workDirectory + "\\Configuration.json";
    }

    public void Read()
    {
        if (!File.Exists(_fileName))
            Write();

        string text = File.ReadAllText(_fileName);
        Configuration configuration = JsonConvert.DeserializeObject<Configuration>(text)!;

        Generate = configuration.Generate;
        Rename = configuration.Rename;
        DelimiterCSV = configuration.DelimiterCSV;

        Write();
    }

    public void Write()
    {
        string text = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(_fileName, text);
    }
}
