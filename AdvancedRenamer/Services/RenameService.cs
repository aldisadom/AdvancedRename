using AdvancedRenamer.Models;
using Microsoft.VisualBasic.FileIO;

namespace AdvancedRenamer.Services;

internal class RenameService
{
    public List<RenameMap> DataMaps { get; set; } = new();

    private readonly string[] _inputFiles;
    private readonly string[] _outputFiles;

    private readonly Configuration _configuration;

    public RenameService(Configuration configuration, string workDirectory)
    {
        string outPath = workDirectory + "\\output";
        string filePath = workDirectory + "\\input";
        _configuration = configuration;

        _inputFiles = Directory.GetFiles(filePath);
        _outputFiles = _inputFiles.Select(f => f.Replace(filePath, outPath)).ToArray();
        GenerateMap(workDirectory);
    }
    public RenameService(Configuration configuration, string[] inputFiles, string[] outputFiles)
    {
        _configuration = configuration;

        _inputFiles = inputFiles;
        _outputFiles = outputFiles;
    }

    private void GenerateMap(string workDirectory)
    {
        var path = workDirectory + "\\Map.csv";
        using (TextFieldParser csvParser = new(path))
        {
            csvParser.CommentTokens = ["#"];
            csvParser.SetDelimiters([_configuration.DelimiterCSV]);
            csvParser.HasFieldsEnclosedInQuotes = true;
            
            while (!csvParser.EndOfData)
            {
                // Read current line fields, pointer moves to the next line.
                string[] fields = csvParser.ReadFields()!;

                DataMaps.Add(new RenameMap () { From = fields[0], To = fields[1] });
            }
        }
    }

    private void RenameFile(string inFile, string outFile)
    {
        string[] fileData = File.ReadAllLines(inFile);
        string[] outData = new string[fileData.Length];

        for (int i = 0; i < fileData.Length; i++)
        {
            outData[i] = fileData[i];
            foreach (RenameMap map in DataMaps)
                outData[i] = outData[i].Replace(map.From, map.To);
        }

        File.WriteAllLines(outFile, outData);
    }

    private void RenameFiles(string[] inFiles, string[] outFiles)
    {
        for (int i = 0; i < inFiles.Length; i++)
            RenameFile(inFiles[i], outFiles[i]);
    }

    public void Rename()
    {
        RenameFiles(_inputFiles, _outputFiles);
    }
}
