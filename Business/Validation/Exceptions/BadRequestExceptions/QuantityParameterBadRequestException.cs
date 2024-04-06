namespace Business.Validation.Exceptions.BadRequestExceptions
{
    public sealed class QuantityParameterBadRequestException : BadRequestException
    {
        public QuantityParameterBadRequestException()
        : base("Parameter quantity must be greater than 0.")
        {
        }
    }
}