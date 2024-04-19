using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedRenamer;

public class GenerateData
{
    public List<string> Data { get; set; }
    public GenerateData(string[] data)
    {
        Data = data.ToList();
    }
}
