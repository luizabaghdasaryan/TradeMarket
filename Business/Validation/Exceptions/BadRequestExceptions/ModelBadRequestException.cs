namespace Business.Validation.Exceptions.BadRequestExceptions
{
    public class ModelBadRequestException : BadRequestException
    {
        public ModelBadRequestException(string message)
            : base(message)
        {
        }
    }
}
