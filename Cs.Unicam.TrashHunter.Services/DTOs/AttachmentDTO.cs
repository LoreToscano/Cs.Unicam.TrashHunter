using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Models.Exceptions;

namespace Cs.Unicam.TrashHunter.Services.DTOs
{
    public class AttachmentDTO
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }

        public int? AttachToPostId { get; set; }
        public string? AttachToCompanyCode { get; set; }
        public bool IsLogo { get; set; }
        public FileType FileType => FileTypeMethods.GetFileTypeFromName(FileName);

        public string Base64Content => System.Convert.ToBase64String(File);
        public AttachmentDTO(Attachment attachment, byte[] file) : this(
                file,
                attachment.FileName,
                attachment.Description,
                attachment.Path,
                attachment.PostId,
                attachment.CompanyCode ?? attachment.CompanyCode,
                attachment.IsLogo()
            )
        {
        }

        public AttachmentDTO(byte[] file, string filename, string description, string path, int? postId, string? companyCode, bool? isLogo) 
        {
            File = file;
            FileName = filename;
            Description = description;
            Path = path;
            AttachToPostId = postId;
            AttachToCompanyCode = companyCode;
            IsLogo = isLogo ?? false;
        }

        internal Attachment ToEntity()
        {
            if (AttachToPostId.HasValue)
                return new Attachment(FileName, Description, Path, AttachToPostId.Value);
            else if (AttachToCompanyCode != null)
                return new Attachment(FileName, Description, Path, AttachToCompanyCode, IsLogo);
            else
                throw new UserException($"Impossibile generare un file senza collegamenti");
        }
    }
}