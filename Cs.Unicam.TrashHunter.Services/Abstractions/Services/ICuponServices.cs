using Cs.Unicam.TrashHunter.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("Cs.Unicam.TrashHunter.Tests")]
namespace Cs.Unicam.TrashHunter.Services.Abstractions.Services
{
    internal interface ICuponServices
    {
        public Task<bool> AddCupon(string companyCode, string userCode, DateTime expirationDate);
        public Task<bool> UseCupon(Guid cuponCode);
        public Task<bool> DeleteCupon(Guid cuponCode);
        public Task<IList<CuponDTO>> GetCupons(string userCode);
        public Task<IList<CuponDTO>> GetCuponsForCompany(string companyCode);
        public Task<CuponDTO> GetCupon(Guid cuponCode);
    }
}
