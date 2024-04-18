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

        service.Rename();

        Console.WriteLine("Renamer finished");
    }
}
