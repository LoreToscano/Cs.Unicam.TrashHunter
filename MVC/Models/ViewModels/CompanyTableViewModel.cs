using Cs.Unicam.TrashHunter.Models.Repositorys.Models;
using Cs.Unicam.TrashHunter.Services.DTOs;

namespace Cs.Unicam.TrashHunter.MVC.Models.ViewModels
{
    public class CompanyTableViewModel
    {
        public IEnumerable<CompanyDTO> Companies { get; set; }

    }
}
