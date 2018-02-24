using System;

namespace Sweesh.Core.Models
{
    public class Config
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AppId { get; set; }
        public ConfigItem[] Configs { get; set; }


        public Config()
        {
            
        }
    }
}
