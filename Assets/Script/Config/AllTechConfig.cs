using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AllTechConfig : ConfigBase
{
    public Dictionary<int, TechConfig> tech = new Dictionary<int, TechConfig>();
}


public class TechConfig
{
    public int ID;
    public string techName;
    public string techDesc;
    public int cost;
}
