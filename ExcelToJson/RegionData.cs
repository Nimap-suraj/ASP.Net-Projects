using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToJson
{
    public class RegionData
    {
        public string RegionName {  get; set; } = string.Empty;
        public string Manager {  get; set; } = string.Empty;
        public Dictionary<string,List<string>> Districts { get; set; } = new Dictionary<string,List<string>>();
    }
}
