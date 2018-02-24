using System;

namespace Sweesh.Core.Models
{
    public class File
    {
        public string FileName { get; set; }
        public string[] PossiblePaths { get; set; }
        public string Description { get; set; }
        public string OS { get; set; }

        public File() 
        {
            
        }
    }
}