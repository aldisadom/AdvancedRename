using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedRenamer;

public class Configuration
{
    public bool Generate { get; set; } = false;
    public bool Rename { get; set; } = true;
    public TemplateName Template { get; set; }
}
