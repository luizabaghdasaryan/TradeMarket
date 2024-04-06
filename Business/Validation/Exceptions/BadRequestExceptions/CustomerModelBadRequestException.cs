using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validation.Exceptions.BadRequestExceptions
{
    public sealed class CustomerModelBadRequestException : ModelBadRequestException
    {
        public CustomerModelBadRequestException(string message)
            : base(message)
        {
        }
    }
}