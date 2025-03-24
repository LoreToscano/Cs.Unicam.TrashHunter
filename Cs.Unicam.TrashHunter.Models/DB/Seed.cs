using Cs.Unicam.TrashHunter.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.DB
{
    public static class Seed
    {
        public static void SeedGenerico(this TrashHunterContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User("admin", "admin", "admin@hunter.com", "password", "s", Entities.Role.Admin, "Tolentino"),
                    new User("Lorenzo", "Toscano", "lorenzo.toscano@hunter.com", "password", "s", Entities.Role.User, "Tolentino"),
                    new User("Controllore", "Controllore", "controllore@hunter.com", "password", "s", Entities.Role.Checker, "Tolentino"),
                    new User("Mario", "Rossi", "mario.rossi@hunter.com", "password", "s", Entities.Role.User, "Tolentino"),
                    new User("Giulia", "Bianchi", "giulia.bianchi@hunter.com", "password", "s", Entities.Role.User, "Tolentino"),
                    new User("Andrea", "Verdi", "andrea.verdi@hunter.com", "password", "s", Entities.Role.User, "Tolentino"),
                    new User("Sara", "Neri", "sara.neri@hunter.com", "password", "s", Entities.Role.User, "Tolentino")
                };

                context.Users.AddRange(users);
                context.SaveChanges();

                var posts = new List<Post>
                {
                    new Post("Spazzatura", "Molta spazzatura in via farnese", "Tolentino", users.First(u => u.Name == "Lorenzo"), new List<Attachment>()),
                    new Post("Rifiuti ingombranti", "Rifiuti ingombranti abbandonati in via Roma", "Tolentino", users.First(u => u.Name == "Mario"), new List<Attachment>()),
                    new Post("Cartacce", "Cartacce sparse nel parco", "Tolentino", users.First(u => u.Name == "Mario"), new List<Attachment>()),
                    new Post("Bottiglie", "Bottiglie di plastica abbandonate", "Tolentino", users.First(u => u.Name == "Giulia"), new List<Attachment>()),
                    new Post("Rifiuti elettronici", "Rifiuti elettronici abbandonati", "Tolentino", users.First(u => u.Name == "Andrea"), new List<Attachment>()),
                    new Post("Siringhe", "Siringhe trovate nel parco", "Tolentino", users.First(u => u.Name == "Sara"), new List<Attachment>()),
                    new Post("Rifiuti organici", "Rifiuti organici non raccolti", "Tolentino", users.First(u => u.Name == "Lorenzo"), new List<Attachment>()),
                    new Post("Rifiuti pericolosi", "Rifiuti pericolosi abbandonati", "Tolentino", users.First(u => u.Name == "Mario"), new List<Attachment>())
                };  

                context.Posts.AddRange(posts);
                context.SaveChanges();
            }


            if (!context.Companies.Any())
            {
                var companies = new List<Company>
                {
                    new Company("COMP001", "EcoClean", "Azienda di pulizia ecologica", new Attachment("logo1.png", "Logo EcoClean", "/images/logo1.png", "COMP001", true), new Attachment("advice1.png", "Immagine pubblicitaria EcoClean", "/images/advice1.png", "COMP001", false)),
                    new Company("COMP002", "GreenWaste", "Gestione dei rifiuti verdi", new Attachment("logo2.png", "Logo GreenWaste", "/images/logo2.png", "COMP002", true), new Attachment("advice2.png", "Immagine pubblicitaria GreenWaste", "/images/advice2.png", "COMP002", false)),
                    new Company("COMP003", "RecycleIt", "Servizi di riciclaggio", new Attachment("logo3.png", "Logo RecycleIt", "/images/logo3.png", "COMP003", true), new Attachment("advice3.png", "Immagine pubblicitaria RecycleIt", "/images/advice3.png", "COMP003", false)),
                    new Company("COMP004", "CleanCity", "Pulizia urbana", new Attachment("logo4.png", "Logo CleanCity", "/images/logo4.png", "COMP004", true), new Attachment("advice4.png", "Immagine pubblicitaria CleanCity", "/images/advice4.png", "COMP004", false)),
                    new Company("COMP005", "WasteAway", "Rimozione rifiuti", new Attachment("logo5.png", "Logo WasteAway", "/images/logo5.png", "COMP005", true), new Attachment("advice5.png", "Immagine pubblicitaria WasteAway", "/images/advice5.png", "COMP005", false)),
                    new Company("COMP006", "EcoRecycle", "Riciclaggio ecologico", new Attachment("logo6.png", "Logo EcoRecycle", "/images/logo6.png", "COMP006", true), new Attachment("advice6.png", "Immagine pubblicitaria EcoRecycle", "/images/advice6.png", "COMP006", false)),
                    new Company("COMP007", "GreenCity", "Servizi verdi urbani", new Attachment("logo7.png", "Logo GreenCity", "/images/logo7.png", "COMP007", true), new Attachment("advice7.png", "Immagine pubblicitaria GreenCity", "/images/advice7.png", "COMP007", false)),
                    new Company("COMP008", "CleanEarth", "Pulizia ambientale", new Attachment("logo8.png", "Logo CleanEarth", "/images/logo8.png", "COMP008", true), new Attachment("advice8.png", "Immagine pubblicitaria CleanEarth", "/images/advice8.png", "COMP008", false)),
                    new Company("COMP009", "WasteNot", "Gestione rifiuti", new Attachment("logo9.png", "Logo WasteNot", "/images/logo9.png", "COMP009", true), new Attachment("advice9.png", "Immagine pubblicitaria WasteNot", "/images/advice9.png", "COMP009", false)),
                    new Company("COMP010", "EcoFriendly", "Servizi eco-friendly", new Attachment("logo10.png", "Logo EcoFriendly", "/images/logo10.png", "COMP010", true), new Attachment("advice10.png", "Immagine pubblicitaria EcoFriendly", "/images/advice10.png", "COMP010", false))
                };

                context.Companies.AddRange(companies);
                context.SaveChanges();

                var cupons = new List<Cupon>
                {
                    new Cupon(companies.First(c => c.CompanyCode == "COMP001"), context.Users.First(u => u.Email == "lorenzo.toscano@hunter.com"), DateTime.Now.AddMonths(1)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP002"), context.Users.First(u => u.Email == "giulia.bianchi@hunter.com"), DateTime.Now.AddMonths(2)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP003"), context.Users.First(u => u.Email == "andrea.verdi@hunter.com"), DateTime.Now.AddMonths(3)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP004"), context.Users.First(u => u.Email == "sara.neri@hunter.com"), DateTime.Now.AddMonths(1)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP005"), context.Users.First(u => u.Email == "mario.rossi@hunter.com"), DateTime.Now.AddMonths(2)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP006"), context.Users.First(u => u.Email == "controllore@hunter.com"), DateTime.Now.AddMonths(3)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP007"), context.Users.First(u => u.Email == "lorenzo.toscano@hunter.com"), DateTime.Now.AddMonths(1)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP008"), context.Users.First(u => u.Email == "giulia.bianchi@hunter.com"), DateTime.Now.AddMonths(2)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP009"), context.Users.First(u => u.Email == "andrea.verdi@hunter.com"), DateTime.Now.AddMonths(3)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP010"), context.Users.First(u => u.Email == "sara.neri@hunter.com"), DateTime.Now.AddMonths(1)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP001"), context.Users.First(u => u.Email == "mario.rossi@hunter.com"), DateTime.Now.AddMonths(2)),
                    new Cupon(companies.First(c => c.CompanyCode == "COMP002"), context.Users.First(u => u.Email == "controllore@hunter.com"), DateTime.Now.AddMonths(3))
                };

                context.Cupons.AddRange(cupons);
                context.SaveChanges();
            }
        }


    }
}
