namespace AdvancedRename.Models;

public class RenameConfiguration
{
    public bool RenameFiles { get; set; } = false;
    public bool RenameContent { get; set; } = true;
}

public enum Options
{
    Generate,
    Rename
}

public class ConfigurationSettings
{
    public RenameConfiguration Rename { get; set; } = new();
    public Options Options { get; set; } = Options.Rename;
    public string DelimiterCSV { get; set; } = ",";

    public string AddComments()
    {
        return "#Options are:\r\n" +
                "#Generate (to generate files from templates)\r\n" +
                "#Rename(rename files/content)\r\n" +
                "\r\n" +
                "#RenameFiles does not work for Renaming function\r\n";
    }
}
