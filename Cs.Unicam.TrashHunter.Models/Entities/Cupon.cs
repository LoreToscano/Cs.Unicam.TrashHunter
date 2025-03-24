using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Entities
{
    public class Cupon
    {
        public Guid CuponId { get; private set; }
        public string CompanyCode { get; private set; }
        public virtual Company Company { get; private set; }
        public string UserCode { get; private set; }
        public virtual User User { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool IsUsed { get; private set; }
        /// <summary>
        /// EF constructor
        /// </summary>
        public Cupon() { }

        public Cupon(Company company, User user, DateTime expirationDate)
        {
            CuponId = Guid.NewGuid();
            Company = company;
            User = user;
            ExpirationDate = expirationDate;
        }

        public Cupon(string companyCode, string userCode, DateTime expirationDate, DateTime creationDate, bool isUsed)
        {
            this.CuponId = Guid.NewGuid();
            CompanyCode = companyCode;
            UserCode = userCode;
            CreationDate = creationDate;
            ExpirationDate = expirationDate;
            IsUsed = isUsed;
        }

        public void Use()
        {
            IsUsed = true;
        }
    }
}
