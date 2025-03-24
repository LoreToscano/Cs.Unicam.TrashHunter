using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.DTOs;

namespace Cs.Unicam.TrashHunter.Services.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly IAttachmentService _attachmentService;
        public CompanyService(IRepository<Company> companyRepository, IAttachmentService attachmentService)
        {
            _companyRepository = companyRepository;
            _attachmentService = attachmentService;
        }

        public async Task AddCompany(CompanyDTO companyDTO)
        {
            var logo = await _attachmentService.Attach(companyDTO.Logo, companyDTO.CompanyCode, true);
            var adviceImage = await _attachmentService.Attach(companyDTO.AdviceImage, companyDTO.CompanyCode, false);

            var company = new Company(
                companyDTO.CompanyCode,
                companyDTO.Name,
                companyDTO.Description,
                logo.ToEntity(),
                adviceImage.ToEntity()
            );

            _companyRepository.Add(company);
            await _companyRepository.Save();
        }

        public async Task<bool> DeleteCompany(string companyCode)
        {
            var company = await _companyRepository.Find(companyCode);
            if (company == null)
            {
                return false;
            }

            await _attachmentService.DeleteAttachment(company.Logo.FileName);
            await _attachmentService.DeleteAttachment(company.AdviceImage.FileName);

            _companyRepository.Delete(company);
            await _companyRepository.Save();
            return true;
        }

        public async Task<CompanyDTO> EditCompany(CompanyDTO companyDTO)
        {
            var company = await _companyRepository.Find(companyDTO.CompanyCode);
            if (company == null)
            {
                throw new KeyNotFoundException("Company not found");
            }

            var logo = await _attachmentService.Attach(companyDTO.Logo, companyDTO.CompanyCode, true);
            var adviceImage = await _attachmentService.Attach(companyDTO.AdviceImage, companyDTO.CompanyCode, false);

            var updatedCompany = new Company(
                company.CompanyCode,
                companyDTO.Name,
                companyDTO.Description,
                logo.ToEntity(),
                adviceImage.ToEntity()
            );

            _companyRepository.Update(updatedCompany);
            await _companyRepository.Save();

            return new CompanyDTO(updatedCompany, logo, adviceImage);
        }

        public async Task<IEnumerable<CompanyDTO>> GetCompanies()
        {
            var companies = await _companyRepository.GetAll();
            var companyDTOs = new List<CompanyDTO>();

            foreach (var company in companies)
            {
                //var logo = await _attachmentService.GetAttachment(company.Logo.FileName);
                //var adviceImage = await _attachmentService.GetAttachment(company.AdviceImage.FileName);
                companyDTOs.Add(new CompanyDTO(company, null, null));
            }

            return companyDTOs;
        }

        public async Task<CompanyDTO> GetCompany(string companyCode)
        {
            var company = await _companyRepository.Find(companyCode);
            if (company == null)
            {
                throw new KeyNotFoundException("Company not found");
            }

            var logo = await _attachmentService.GetAttachment(company.Logo.FileName);
            var adviceImage = await _attachmentService.GetAttachment(company.AdviceImage.FileName);

            return new CompanyDTO(company, logo, adviceImage);
        }
    }
}
