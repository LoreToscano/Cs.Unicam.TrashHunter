using Cs.Unicam.TrashHunter.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Entities
{
    public class Company
    {
        public string CompanyCode { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public virtual Attachment Logo { get; private set; }
        public virtual Attachment AdviceImage { get; private set; }
        public virtual ICollection<Cupon> Cupons { get; private set; }
        public virtual ICollection<Post> Posts { get; private set; }
        /// <summary>
        /// EF Constructor
        /// </summary>
        public Company() { }

        public Company(string companyCode, string name, string description, Attachment logo, Attachment adviceImage)
        {
            if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(name))
                throw new UserException("Parametri non validi");
            if (!logo.IsImage() || !adviceImage.IsImage())
                throw new UserException($"uno dei file non è un immagine oppure non è supportata dal sistema {logo}, {adviceImage}");
            CompanyCode = companyCode;
            Name = name;
            Description = description;
            Logo = logo;
            AdviceImage = adviceImage;
        }


        private bool CheckImage(string imageName)
        {
            return FileTypeMethods.GetFileTypeFromName(imageName).IsImage();
        }
    }
}
