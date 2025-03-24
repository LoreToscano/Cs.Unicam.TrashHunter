using Cs.Unicam.TrashHunter.Models.Abstractions.Repositorys;
using Cs.Unicam.TrashHunter.Models.Entities;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Cs.Unicam.TrashHunter.Services.DTOs;
using Cs.Unicam.TrashHunter.Services.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("Cs.Unicam.TrashHunter.Services.Tests")]
namespace Cs.Unicam.TrashHunter.Services.Services
{
    internal class CuponServices : ICuponServices
    {
        private readonly IRepository<Cupon> _cuponRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<User> _userRepository;

        public CuponServices(IRepository<Cupon> cuponRepository, IRepository<Company> companyRepository, IRepository<User> userRepository)
        {
            _cuponRepository = cuponRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> AddCupon(string companyCode, string userCode, DateTime expirationDate)
        {
            var company = await _companyRepository.Find(companyCode);
            if (company == null)
            {
                throw new KeyNotFoundException("Company not found");
            }

            var user = await _userRepository.Find(userCode);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var cupon = new Cupon(company, user, expirationDate);
            _cuponRepository.Add(cupon);
            await _cuponRepository.Save();
            return true;
        }

        public async Task<bool> DeleteCupon(Guid cuponCode)
        {
            var cupon = await _cuponRepository.Find(cuponCode);
            if (cupon == null)
            {
                return false;
            }

            _cuponRepository.Delete(cupon);
            await _cuponRepository.Save();
            return true;
        }

        public async Task<IList<CuponDTO>> GetCupons(string userCode)
        {
            var filter = new FilterBuilder()
                .AddIdFilter(userCode, nameof(Cupon.UserCode))
                .Build();

            var cupons = await _cuponRepository.GetFiltered(filter, 0, int.MaxValue);
            return cupons.Items.Select(c => new CuponDTO(c)).ToList();
        }

        public async Task<IList<CuponDTO>> GetCuponsForCompany(string companyCode)
        {
            var filter = new FilterBuilder()
                .AddIdFilter(companyCode, nameof(Cupon.CompanyCode))
                .Build();

            var cupons = await _cuponRepository.GetFiltered(filter, 0, int.MaxValue);
            return cupons.Items.Select(c => new CuponDTO(c)).ToList();
        }

        public async Task<CuponDTO> GetCupon(Guid cuponCode)
        {
            var cupon = await _cuponRepository.Find(cuponCode);
            if (cupon == null)
            {
                throw new KeyNotFoundException("Cupon not found");
            }

            return new CuponDTO(cupon);
        }

        public async Task<bool> UseCupon(Guid cuponCode)
        {
            var cupon = await _cuponRepository.Find(cuponCode);
            if (cupon == null)
            {
                return false;
            }

            cupon.Use();
            _cuponRepository.Update(cupon);
            await _cuponRepository.Save();
            return true;
        }
    }
}


