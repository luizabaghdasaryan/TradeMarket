using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validation.Exceptions.BadRequestExceptions
{
    public sealed class CategoryModelBadRequestException : ModelBadRequestException
    {
        public CategoryModelBadRequestException(string message)
            : base(message)
        {
        }
    }
}