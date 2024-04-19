using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedRenamer;

public enum TemplateName
{
    VLVA,
    VLVD,
    MOT,
    DKMOT,
    PID,
    SEQ
}

internal class RenamerService
{
    private List<RenameMap> DataMaps { get; set; } = new ();

    private readonly List<GenerateData> GenerateList = new ();
    private readonly string _workDirectory = Directory.GetCurrentDirectory();
    private readonly string _outDirectory = string.Empty;
    private readonly GenerateData _replaceList;
    private readonly Configuration _configuration;

    public RenamerService(Configuration configuration)
    {
        _outDirectory = _workDirectory + "\\output";
        _replaceList = new GenerateData(["Tag_to_replace", "Operative_to_replace" , "Units_to_replace"]);
        _configuration = configuration;
    }

    private void GenerateListForTemplate()
    {
        GenerateList.Clear();
        GenerateList.Add(new GenerateData(["Tag_to_replace", "Operative_to_replace", "Units_to_replace"]));

        switch (_configuration.Template)
        {
            case TemplateName.VLVA:
                //VLVA
                GenerateList.Add(new GenerateData(["LBG21AA151", "RG-300", ""]));
                GenerateList.Add(new GenerateData(["LAE10AA151", "T-407", ""]));
                break;
            case TemplateName.VLVD:
                //VLVD
                GenerateList.Add(new GenerateData(["NDA50AA002", "T-1405", ""]));
                GenerateList.Add(new GenerateData(["NDB41AA001", "T-1402", ""]));
                GenerateList.Add(new GenerateData(["NDB40AA001", "T-1404", ""]));
                GenerateList.Add(new GenerateData(["NDB20AA001", "T-1401", ""]));
                GenerateList.Add(new GenerateData(["NDB21AA001", "T-1406", ""]));
                GenerateList.Add(new GenerateData(["NDB25AA001", "T-1400", ""]));
                GenerateList.Add(new GenerateData(["NDB22AA001", "T-1407", ""]));
                GenerateList.Add(new GenerateData(["LBG31AA001", "RG-301", ""]));
                GenerateList.Add(new GenerateData(["NDB45AA001", "T-1403", ""]));
                GenerateList.Add(new GenerateData(["HTA85AA001", "DS-51", ""]));
                GenerateList.Add(new GenerateData(["PGB11AA001", "T-1410", ""]));
                GenerateList.Add(new GenerateData(["LAE10AA001", "T-403", ""]));
                GenerateList.Add(new GenerateData(["PGB23AA001", "T-1411", ""]));
                GenerateList.Add(new GenerateData(["NDB32AA001", "T-1409", ""]));
                GenerateList.Add(new GenerateData(["HTQ48AA001", "KKE-636", ""]));
                GenerateList.Add(new GenerateData(["PGB24AA001", "T-1413", ""]));
                GenerateList.Add(new GenerateData(["PGB10AA001", "T-1416", ""]));
                GenerateList.Add(new GenerateData(["HTQ47AA001", "KKE-635", ""]));
                GenerateList.Add(new GenerateData(["PGB30AA001", "T-1415", ""]));
                GenerateList.Add(new GenerateData(["NDB31AA001", "T-1408", ""]));
                GenerateList.Add(new GenerateData(["HTA80AA001", "DS-52", ""]));
                GenerateList.Add(new GenerateData(["PGB31AA001", "T-1412", ""]));
                GenerateList.Add(new GenerateData(["HTA83AA001", "DS-50", ""]));
                GenerateList.Add(new GenerateData(["PGB32AA001", "T-1414", ""]));
                GenerateList.Add(new GenerateData(["GAC90AA001", "TV-47", ""]));
                break;
            case TemplateName.MOT:
                break;
            case TemplateName.DKMOT:
                //MOT DK
                GenerateList.Add(new GenerateData(["HTQ49AP001", "KE-4/II KS-1", ""]));
                GenerateList.Add(new GenerateData(["HTQ50AP001", "KE-4/II KS-2", ""]));
                GenerateList.Add(new GenerateData(["HTC40AN001", "KE-4 D-3", ""]));
                GenerateList.Add(new GenerateData(["LCN12AP001", "AŠS GKS-1", ""]));
                GenerateList.Add(new GenerateData(["HTQ45AP001", "", ""]));
                GenerateList.Add(new GenerateData(["HTQ42AP001", "KE-4/II KIS-2", ""]));
                GenerateList.Add(new GenerateData(["NDC40AP001", "TS-26", ""]));
                GenerateList.Add(new GenerateData(["HTQ41AP001", "KE-4/II KIS-1", ""]));
                GenerateList.Add(new GenerateData(["NDC30AP001", "TS-25", ""]));
                GenerateList.Add(new GenerateData(["PGB24AP001", "AŠS CS-2", ""]));
                GenerateList.Add(new GenerateData(["PGB23AP001", "AŠS CS-1", ""]));
                GenerateList.Add(new GenerateData(["LCN14AP001", "AŠS GKS-2", ""]));
                break;
            case TemplateName.PID:
                GenerateList.Add(new GenerateData(["HTQ43DF001", "", "m³/h"]));
                GenerateList.Add(new GenerateData(["HTQ44DL001", "", "%"]));
                GenerateList.Add(new GenerateData(["HTA83DP001", "", "mbar"]));
                GenerateList.Add(new GenerateData(["HTA83DP001_vlv", "", "mbar"]));
                GenerateList.Add(new GenerateData(["LBG21DP002", "", "bar"]));
                GenerateList.Add(new GenerateData(["LBG30DT002", "", "°C"]));
                GenerateList.Add(new GenerateData(["LCN11DL001", "", "%"]));
                GenerateList.Add(new GenerateData(["PGBx0DT001", "", "°C"]));
                GenerateList.Add(new GenerateData(["PGB10DF001", "", "m³/h"]));
                GenerateList.Add(new GenerateData(["NDB40DP001", "", "bar"]));
                GenerateList.Add(new GenerateData(["NDB4xDT002", "", "°C"]));
                GenerateList.Add(new GenerateData(["NDB41DF001", "", "m³/h"]));
                break;
            case TemplateName.SEQ:
                break;
        }
    }

    private void GenerateMapForTemplate(GenerateData item)
    {
        DataMaps.Clear();

        for (int i = 0; i < item.Data.Count; i++)
        {
            DataMaps.Add(new RenameMap() { From = _replaceList.Data[i], To = item.Data[i] });
        }
    }

    private void GenerateFiles(string pathInput,string[] inputFiles)
    {
        GenerateListForTemplate();

        string directory = _outDirectory + "\\" + _configuration.Template.ToString();
        foreach (GenerateData item in GenerateList)
        {
            GenerateMapForTemplate(item);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string[] outputFiles = inputFiles.Select(f => f.Replace(pathInput, directory)
                                                            .Replace(_replaceList.Data[0], item.Data[0])).ToArray();
            RenameFiles(inputFiles, outputFiles);
        }
    }

    private void RenameFile(string inFile, string outFile)
    {
        string [] fileData = File.ReadAllLines(inFile);
        string[] outData = new string[fileData.Length];

        for (int i = 0; i < fileData.Length; i++)
        {
            outData[i]= fileData[i];
            foreach (var map in DataMaps)
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
        if (_configuration.Generate)
        {
            string pathInput = _workDirectory + "\\templates\\" + _configuration.Template.ToString();

            string[] inputFiles = Directory.GetFiles(pathInput);

            GenerateFiles(pathInput,inputFiles);
        }
        else
        {
            string pathInput = _workDirectory + "\\input";
            string[] inputFiles = Directory.GetFiles(pathInput);

            string[] outputFiles = inputFiles.Select(f => f.Replace(pathInput, _outDirectory)).ToArray();

            RenameFiles(inputFiles, outputFiles);
        }
    }
}
