using Cs.Unicam.TrashHunter.Models.Entities;

namespace Cs.Unicam.TrashHunter.Services.DTOs
{
    public class CuponDTO
    {
        Guid Id { get; set; }
        public string UserCode { get; set; }
        public string CompanyCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }
        public CuponDTO(Cupon cupon)
        {
            Id = cupon.CuponId;
            UserCode = cupon.UserCode;
            CompanyCode = cupon.CompanyCode;
            CreationDate = cupon.CreationDate;
            ExpirationDate = cupon.ExpirationDate;
            IsUsed = cupon.IsUsed;
                
        }
    }
}