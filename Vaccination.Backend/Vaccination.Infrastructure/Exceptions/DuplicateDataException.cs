using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vaccination.Infrastructure.Exceptions
{
    public class DuplicateDataException : Exception
    {
        public DuplicateDataException(string message) : base(message)
        { }
    }
}