using Cs.Unicam.TrashHunter.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Tests.Models
{
    abstract class BaseTest
    {
        TrashHunterContext _context;
        public BaseTest()
        {
            _context = new TrashHunterContext();
        }
    }
}
