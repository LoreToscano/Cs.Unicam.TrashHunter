using Cs.Unicam.TrashHunter.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Entities
{
    public class Attachment
    {
        public string FileName { get; private set; }
        public string Description { get; private set; }
        public string Path { get; private set; }
        public virtual Post? Post { get; private set; }
        public int? PostId { get; private set; }
        public virtual Company? CompanyLogo { get; private set; }
        public string? CompanyLogoCode { get; private set; }
        public string? CompanyCode { get; private set; }
        public virtual Company? Company { get; private set; }

        public Attachment() { }
        public Attachment(string fileName, string description, string path, int postId)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(path))
                throw new UserException("Parametri non validi");
            FileName = fileName;
            Description = description;
            Path = path;
            PostId = postId;    
        }

        public Attachment(string fileName, string description, string path, string companyCode, bool isLogo)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(companyCode))
                throw new UserException("Parametri non validi");
            FileName = fileName;
            Description = description;
            Path = path;
            if (isLogo)
            {
                CompanyLogoCode = companyCode;
            }
            else
            {
                CompanyCode = companyCode;
            }
        }

        public Attachment Edit(string description)
        {
            Description = description;
            return this;
        }

        public bool IsLogo() => CompanyLogoCode != null;
        public bool IsImage() => FileTypeMethods.GetFileTypeFromName(FileName).IsImage();
    }
    public enum FileType
    {
        PDF = 1,
        PNG = 2,
        JPG = 3,
        XLSX = 4,
        TXT = 5
    }

    public static class FileTypeMethods 
    {
        public static bool IsImage(this FileType fileType) => fileType is FileType.JPG or FileType.PNG;

        public static FileType GetFileTypeFromName(string filename) => filename.Split('.').LastOrDefault() switch
        {
            "pdf" => FileType.PDF,
            "png" => FileType.PNG,
            "jpg" => FileType.JPG,
            "xlsx" => FileType.XLSX,
            "txt" => FileType.TXT,
            _ => throw new UserException($"Tipo di file non supportato {filename}")
        };
    }
}
