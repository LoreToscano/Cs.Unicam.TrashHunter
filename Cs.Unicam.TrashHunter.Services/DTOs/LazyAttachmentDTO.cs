using Cs.Unicam.TrashHunter.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.DTOs
{
    /// <summary>
    /// Data Transfer Object for Attachment. This attachment is lazy in the sense that it does not contain the actual file content.
    /// </summary>
    public class LazyAttachmentDTO
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public FileType FileType => FileTypeMethods.GetFileTypeFromName(FileName);

        public LazyAttachmentDTO(Attachment attachment)
        {
            FileName = attachment.FileName;
            Description = attachment.Description;
        }
    }
}
