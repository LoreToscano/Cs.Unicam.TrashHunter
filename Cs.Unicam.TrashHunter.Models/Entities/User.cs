using Cs.Unicam.TrashHunter.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Entities
{
    public class User
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }
        public Role Role { get; private set; }
        public string City { get; private set; }
        public virtual ICollection<Post> Post { get; private set; }
        public virtual ICollection<Post> PostCompleted { get; private set; }
        public virtual ICollection<Cupon> Cupons { get; private set; }
        /// <summary>
        /// Empty constructor for EF
        /// </summary>
        public User()
        {

        }

        public User(string name, string surname, string email, string password, string salt, Role role, string city)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(salt) || string.IsNullOrWhiteSpace(city))
                throw new UserException("Parametri non validi");
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            Salt = salt;
            Role = role;
            City = city;
        }
    }

    public enum Role
    {
        Admin, 
        User,
        Checker
    }

    public static class RoleMethods
    {
        public static string GetRoleString(this Role role)
        {
            return role switch
            {
                Role.Admin => "Amministratore",
                Role.User => "Hunter",
                Role.Checker => "Controllore",
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            };
        }
    }

}
