using Cs.Unicam.TrashHunter.MVC.Models.ViewModels;
using Cs.Unicam.TrashHunter.Services.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cs.Unicam.TrashHunter.MVC.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        public CompanyController(IServiceScopeFactory factory, ICompanyService companyService) : base(factory)
        {
            _companyService = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new CompanyTableViewModel();
            vm.Companies = await _companyService.GetCompanies();
            return View(vm);
        }
    }
}
