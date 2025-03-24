using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.Exceptions
{
    public class UserException : Exception
    {
        private readonly object?[] _args;
        public UserException(string message, params object[] args) : base(string.Format(message, args))
        {
            _args = args;
        }
        public UserException(FormattableString message) : base(message.ToString())
        {
            _args = message.GetArguments();
        }

        
    }
}
