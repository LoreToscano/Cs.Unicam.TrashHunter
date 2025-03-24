using Cs.Unicam.TrashHunter.Services.DTOs;
using System.Security.Cryptography;

namespace Cs.Unicam.TrashHunter.MVC.Models.ViewModels
{
    public class AttachmentViewModel
    {
        public string FileName { get; set; }
        public string Base64Content { get; set; }

        public AttachmentViewModel(AttachmentDTO attachmentDTO)
        {
            FileName = attachmentDTO.FileName;
            if (attachmentDTO.File != null)
                Base64Content = System.Convert.ToBase64String(attachmentDTO.File);
            else
                Base64Content = "";
        }
    }
}
