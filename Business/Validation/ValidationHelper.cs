using Business.Models;
using Business.Validation.Exceptions;
using Business.Validation.Exceptions.BadRequestExceptions;

namespace Business.Validation
{
    internal static class ValidationHelper
    {
        public static void Validate(BaseModel model, bool validateId = false)
        {
            if (model is null)
            {
                throw new ModelBadRequestException("Model is null");
            }

            if (validateId && model.Id <= 0)
            {
                throw new ModelBadRequestException("Model id is invalid");
            }
        }
    }
}