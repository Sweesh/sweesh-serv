using System;
namespace Sweesh.Core.Models
{
    public class ConfigItem
    {
        public string ConfigName { get; set; }
        public Byte[] Raw { get; set; }
        public string destination { get; set; }

        public ConfigItem()
        {
        }
    }
}
