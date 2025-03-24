using Cs.Unicam.TrashHunter.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.Abstractions.Services
{
    public interface ICompanyService
    {
        public Task AddCompany(CompanyDTO companyDTO);
        public Task<CompanyDTO> GetCompany(string companyCode);
        public Task<IEnumerable<CompanyDTO>> GetCompanies();
        public Task<CompanyDTO> EditCompany(CompanyDTO companyDTO);
        public Task<bool> DeleteCompany(string companyCode);
    }
}
