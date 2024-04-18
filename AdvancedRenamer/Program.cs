namespace AdvancedRenamer;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Renamer started");
        Configuration configuration = new()
        {
            Template = TemplateName.VLVD,
            Generate = true
        };

        RenamerService service = new RenamerService(configuration);

        string _outDirectory = Directory.GetCurrentDirectory() + "\\output";
    
        if (Directory.Exists(_outDirectory))
            Directory.Delete(_outDirectory,true);

        Directory.CreateDirectory(_outDirectory);

        foreach (TemplateName template in Enum.GetValues(typeof(TemplateName)))
        {
            configuration.Template = template;
            service.Rename();
        }        

        Console.WriteLine("Renamer finished");
    }
}
