using Cs.Unicam.TrashHunter.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Services.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string City { get; set; }
        public string CompleteName => $"{Name} {Surname}";

        public UserDTO(User user)
        {
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
            Role = user.Role;
            City = user.City;
        }

    }
}
