using AdvancedRenamer.Models;
using Microsoft.VisualBasic.FileIO;

namespace AdvancedRenamer.Services;

internal class GeneratorService
{
    //key is template name, Value is columns that need to be replaced
    private List<GenerateDataList> _generateList;
    private List<string> _replaceList;

    private readonly string _workDirectory;
    private readonly string _outDirectory;
    private readonly Configuration _configuration;

    public GeneratorService(Configuration configuration, string workDirectory)
    {
        _workDirectory = workDirectory;
        _outDirectory = workDirectory + "\\output";
        _configuration = configuration;
    }

    private void GenerateListForTemplate()
    {
        _generateList = new();
        _replaceList = new();

        var path = _workDirectory + "\\Generate.csv";
        using (TextFieldParser csvParser = new(path))
        {
            csvParser.CommentTokens = ["#"];
            csvParser.SetDelimiters([_configuration.DelimiterCSV]);
            csvParser.HasFieldsEnclosedInQuotes = true;
            // Skip the row with the column names
            string[] text = csvParser.ReadFields()!;

            _replaceList = text.Skip(1).ToList();
            while (!csvParser.EndOfData)
            {
                // Read current line fields, pointer moves to the next line.
                string[] fields = csvParser.ReadFields()!;
                string templateName = fields[0];

                GenerateDataList generateDataList = new()
                {
                    Template = templateName,
                    Data = fields.Skip(1).ToList()
                };
                _generateList.Add(generateDataList);
            }
        }
    }

    private void GenerateMapForTemplate(RenameService renameService, GenerateDataList generateDataList)
    {
        for (int i = 0; i < generateDataList.Data.Count; i++)
        {
            renameService.DataMaps.Add(new RenameMap() 
                                    { 
                                        From = _replaceList[i],
                                        To = generateDataList.Data[i]
                                    });
        }
    }

    private void GenerateFiles(string pathInput, string template)
    {
        string directory = _outDirectory + "\\" + template;
        string[] inputFiles = Directory.GetFiles(pathInput);

        foreach (GenerateDataList generateDataList in _generateList)
        {
            if (generateDataList.Template != template)
                continue;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string[] outputFiles = inputFiles.Select(f => f.Replace(pathInput, directory)
                                                            .Replace(_replaceList[0], generateDataList.Data[0])).ToArray();
            if (_configuration.Rename)
            {
                RenameService service = new(_configuration, inputFiles,outputFiles);
                GenerateMapForTemplate(service, generateDataList);
                service.Rename();
            }
            else
            {
                for (int i = 0; i < inputFiles.Length; i++)
                    File.Copy(inputFiles[i], outputFiles[i]);
            }
        }
    }

    public void Generate()
    {
        GenerateListForTemplate();
        string pathInput;

        List <string> templates = _generateList.Select(l => l.Template).Distinct().ToList();
        foreach (string template in templates)
        {
            pathInput = _workDirectory + "\\templates\\" + template;
            GenerateFiles(pathInput, template);
        }
    }
}
