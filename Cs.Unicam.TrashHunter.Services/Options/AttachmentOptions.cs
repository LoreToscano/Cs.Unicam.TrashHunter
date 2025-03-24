using Cs.Unicam.TrashHunter.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Options
{
    public class AttachmentOptions
    {
        public string RootPath { get; set; }
        internal long MaxSize { get; set; }
        internal FileType[] Exlude { get; set; }
        
        public AttachmentOptions() { }
        public AttachmentOptions(string rootPath, long maxSize, FileType[] exclude)
        {
            RootPath = rootPath;
            MaxSize = maxSize;
            Exlude = exclude;
        }

    }
}
