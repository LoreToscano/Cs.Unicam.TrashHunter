using Cs.Unicam.TrashHunter.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.DTOs
{
    public class CompanyDTO
    {
        public string CompanyCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AttachmentDTO Logo { get; set; }
        public AttachmentDTO AdviceImage { get; set; }
        public ICollection<CuponDTO> Cupons { get; set; }
        public ICollection<PostDTO> Posts { get; set; }
        /// <summary
        /// For testing 
        /// </summary>
        public CompanyDTO()
        {
            Cupons = new List<CuponDTO>();
            Posts = new List<PostDTO>();
        }
        public CompanyDTO(Company company, AttachmentDTO logo, AttachmentDTO adviceImage)
        {
            CompanyCode = company.CompanyCode;
            Name = company.Name;
            Description = company.Description;
            Logo = logo;
            AdviceImage = adviceImage;
            Cupons = company.Cupons?.Select(c => new CuponDTO(c)).ToList() ?? [];
            Posts = company.Posts?.Select(p => new PostDTO(p, null)).ToList() ?? [];
        }
    }
}